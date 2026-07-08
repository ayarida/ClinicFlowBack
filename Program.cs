using System.Text.Json.Serialization;
using ClinicFlow.API.Middleware;
using ClinicFlow.API.Services;
using ClinicFlow.Application;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentClinicService, CurrentClinicService>();
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Model-binding failures (e.g. an invalid enum string in the request body) never
// throw — they short-circuit via [ApiController]'s automatic 400 before reaching
// ExceptionHandlingMiddleware. Reshape that response to match it: application/json
// with { title, status, errors }, not the default application/problem+json.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

        return new JsonResult(new { title = "Validation failed.", status = StatusCodes.Status400BadRequest, errors })
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentType = "application/json"
        };
    };
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
