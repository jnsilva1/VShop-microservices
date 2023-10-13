using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.Services;

public class ProfileAppService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

    public ProfileAppService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
#pragma warning disable 8600, 8604
        string id = context.Subject.GetSubjectId();
        ApplicationUser user = await _userManager.FindByIdAsync(id);

        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

        if (_userManager.SupportsUserRole)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user: user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));

                if (_roleManager.SupportsRoleClaims)
                {
                    IdentityRole? identityRole = await _roleManager.FindByNameAsync(roleName: role);

                    if (identityRole is not null)
                        claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                }
            }
        }

        context.IssuedClaims = claims;
#pragma warning restore 8600, 8604
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string userId = context.Subject.GetSubjectId();

        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        context.IsActive = user is not null;
    }
}
