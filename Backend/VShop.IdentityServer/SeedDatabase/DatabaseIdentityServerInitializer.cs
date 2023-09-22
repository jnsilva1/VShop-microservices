using System;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Configuration;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.SeedDatabase;

public class DatabaseIdentityServerInitializer : IDatabaseSeedInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseIdentityServerInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeSeedRoles()
    {
        if (!_roleManager.RoleExistsAsync(roleName: IdentityConfiguration.Admin).Result)
        {
            IdentityRole roleAdmin = new()
            {
                Name = IdentityConfiguration.Admin,
                NormalizedName = IdentityConfiguration.Admin.ToUpper()
            };

            _roleManager.CreateAsync(role: roleAdmin).Wait();
        }

        if (!_roleManager.RoleExistsAsync(roleName: IdentityConfiguration.Client).Result)
        {
            IdentityRole roleClient = new()
            {
                Name = IdentityConfiguration.Client,
                NormalizedName = IdentityConfiguration.Client.ToUpper()
            };

            _roleManager.CreateAsync(role: roleClient).Wait();
        }
    }

    public void InitializeSeedUsers()
    {
        if (_userManager.FindByEmailAsync(email: "admin1@com.br").Result is null)
        {
            ApplicationUser admin = new()
            {
                UserName = "admin1",
                NormalizedUserName = "ADMIN1",
                Email = "admin1@com.br",
                NormalizedEmail = "ADMIN1@COM.BR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 91234-5678",
                FirstName = "Usuario",
                LastName = "Admin1",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult resultAdmin = _userManager.CreateAsync(user: admin, password: "Numsey#2023").Result;
            if (resultAdmin.Succeeded)
            {
                _userManager.AddToRoleAsync(user: admin, role: IdentityConfiguration.Admin).Wait();

                var adminClaims = _userManager.AddClaimsAsync(user: admin, claims: new Claim[]{
                    new (type: JwtClaimTypes.Name, value: $"{admin.FirstName} {admin.LastName}"),
                    new (type: JwtClaimTypes.GivenName, value: admin.FirstName),
                    new (type: JwtClaimTypes.FamilyName, value: admin.LastName),
                    new (type: JwtClaimTypes.Role, value: IdentityConfiguration.Admin)
                });
            }
        }

        if (_userManager.FindByEmailAsync(email: "client1@com.br").Result is null)
        {
            ApplicationUser client = new()
            {
                UserName = "Client1",
                NormalizedUserName = "CLIENT1",
                Email = "client1@com.br",
                NormalizedEmail = "CLIENT1@COM.BR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 91234-5678",
                FirstName = "Usuario",
                LastName = "Client1",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult resultClient = _userManager.CreateAsync(user: client, password: "Numsey#2023").Result;
            if (resultClient.Succeeded)
            {
                _userManager.AddToRoleAsync(user: client, role: IdentityConfiguration.Client).Wait();

                var clientClaims = _userManager.AddClaimsAsync(user: client, claims: new Claim[]{
                    new (type: JwtClaimTypes.Name, value: $"{client.FirstName} {client.LastName}"),
                    new (type: JwtClaimTypes.GivenName, value: client.FirstName),
                    new (type: JwtClaimTypes.FamilyName, value: client.LastName),
                    new (type: JwtClaimTypes.Role, value: IdentityConfiguration.Client)
                });
            }
        }

    }
}
