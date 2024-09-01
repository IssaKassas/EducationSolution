using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebApp.Data;

namespace WebApp.Middleware
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options )
        : base(userManager, roleManager, options) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.first_Name ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.last_Name ?? ""));
            identity.AddClaim(new Claim("UserId", user.UserId.ToString()));
            identity.AddClaim(new Claim("Approved", user.Approved.ToString()));

            if (user.TwoFactorEnabled) identity.AddClaim(new Claim("TwoFactorEnabled", "true"));

            else
            {
                if (user.TwoFactorEmail == 1)
                {
                    identity.AddClaim(new Claim("TwoFactorEmail", "true"));
                    identity.AddClaim(new Claim("TwoFactorEnabled", "true"));
                }

                else identity.AddClaim(new Claim("TwoFactorEnabled", "false"));
            }

            return identity;
        }
    }
}
