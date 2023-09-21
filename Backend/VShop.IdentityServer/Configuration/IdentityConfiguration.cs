using System;
using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace VShop.IdentityServer.Configuration;

public class IdentityConfiguration
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>{
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>{
            new(name: "vshop", displayName: "VShop Server"),
            new(name: "read", displayName: "Read Data"),
            new(name: "write", displayName: "Write Data"),
            new(name: "delete", displayName: "Delete Data")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>{
            new(){
                ClientId = "client",
                ClientSecrets = { new Secret("abracadabra#simsalabim".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read", "write", "profile"}
            },
            new(){
                ClientId =  "vshop",
                ClientSecrets = { new Secret("abracadabra#simsalabim".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:7103/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:7103/signout-callback-oidc"},
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "vshop"
                }

            }

        };

}
