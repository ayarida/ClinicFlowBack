using System.Security.Claims;
using ClinicFlow.Domain.Interfaces.Repositories;

namespace ClinicFlow.API.Middleware;

public class TenantMiddleware(RequestDelegate next, IConfiguration configuration, IHostEnvironment env)
{
    public const string ClinicIdKey = "ClinicId";
    public const string SystemOwnerRole = "SystemOwner";

    public async Task InvokeAsync(HttpContext context, IStaffRepository staffRepository)
    {
        var email = context.User.FindFirstValue("preferred_username")
            ?? context.User.FindFirstValue(ClaimTypes.Email);

        if (!string.IsNullOrEmpty(email))
        {
            if (IsSystemOwnerEmail(email))
                context.User.AddIdentity(new ClaimsIdentity([new Claim(ClaimTypes.Role, SystemOwnerRole)]));

            var staff = await staffRepository.GetByEmailAsync(email, context.RequestAborted);
            if (staff is not null && staff.IsActive)
                context.Items[ClinicIdKey] = staff.ClinicId;
        }
        else if (env.IsDevelopment())
        {
            // Dev bypass: used when Azure AD is not yet configured.
            // Set DevClinicId in appsettings.Development.json to match a real clinic row.
            var devId = configuration["DevClinicId"];
            if (Guid.TryParse(devId, out var devClinicId))
                context.Items[ClinicIdKey] = devClinicId;

            // Dev-only stand-in for the JWT preferred_username claim: the frontend's
            // /owner entry screen sends this header instead of a token. Checked against
            // the same SystemOwnerEmails list as the real JWT path above — this is a
            // real per-email check, not a blanket bypass. context.User is replaced (not
            // just AddIdentity) because with no JWT, the primary identity is
            // unauthenticated, and [Authorize]'s anonymous check looks at that identity.
            var devOwnerEmail = context.Request.Headers["X-Dev-Owner-Email"].FirstOrDefault();
            if (!string.IsNullOrEmpty(devOwnerEmail) && IsSystemOwnerEmail(devOwnerEmail))
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.Role, SystemOwnerRole), new Claim(ClaimTypes.Email, devOwnerEmail)],
                    authenticationType: "DevHeader"));
            }
        }

        await next(context);
    }

    private bool IsSystemOwnerEmail(string email)
    {
        var systemOwnerEmails = configuration.GetSection("SystemOwnerEmails").Get<string[]>() ?? [];
        return systemOwnerEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
    }
}
