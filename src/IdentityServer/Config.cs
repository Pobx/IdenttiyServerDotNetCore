// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer {
  public static class Config {
    private static string spaClientUrl = "https://localhost:44311";
    public static IEnumerable<IdentityResource> IdentityResources =>
      new IdentityResource[] {
        new IdentityResources.OpenId ()
      };

    public static IEnumerable<ApiScope> ApiScopes =>
      new ApiScope[] {
        new ApiScope ("api1", "My API")
      };

    public static IEnumerable<Client> Clients =>
      new Client[] {
        new Client {
        ClientId = "client",
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets = {
        new Secret ("secret".Sha256 ())
        },
        AllowedScopes = { "api1" }
        },

        new Client {
        ClientId = "spaCodeClient",
        ClientName = "SPA Code Client",
        AccessTokenType = AccessTokenType.Jwt,
        AccessTokenLifetime = 120, // 2 minuite
        IdentityTokenLifetime = 60,

        RequireClientSecret = false,
        AllowedGrantTypes = GrantTypes.Code,
        RequirePkce = true,

        AllowAccessTokensViaBrowser = true,
        RedirectUris = new List<string> {
        $"{spaClientUrl}/callback",
        $"{spaClientUrl}/silent-renew.html",
        "https://localhost:4200",
        "https://localhost:4200/silent-renew.html"
        },
        PostLogoutRedirectUris = new List<string> {
        $"{spaClientUrl}/unauthorized",
        $"{spaClientUrl}",
        "https://localhost:4200/unauthorized",
        "https://localhost:4200"
        },
        AllowedCorsOrigins = new List<string> {
        $"{spaClient}",
        "https://localhost:4200"
        },
        AllowedScopes = new List<string> {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        "resourceApi"
        }

        }
      };
  }
}