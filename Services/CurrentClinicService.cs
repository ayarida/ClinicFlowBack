using System.Security.Claims;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.API.Middleware;

namespace ClinicFlow.API.Services;

public class CurrentClinicService : ICurrentClinicService
{
    public Guid ClinicId { get; }
    public string UserEmail { get; }

    public CurrentClinicService(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No HTTP context available.");

        if (context.Items.TryGetValue(TenantMiddleware.ClinicIdKey, out var value) && value is Guid id)
            ClinicId = id;
        else
            throw new UnauthorizedAccessException("Clinic context could not be resolved.");

        UserEmail = context.User.FindFirstValue("preferred_username")
            ?? context.User.FindFirstValue(ClaimTypes.Email)
            ?? string.Empty;
    }
}
