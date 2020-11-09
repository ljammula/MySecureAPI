// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdServer.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(), 
                new IdentityResources.Address(), 
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>(){"role"}
                ),
                new IdentityResource(
                    "country",
                    "The country you're living in",
                    new List<string>() { "country" }),
                new IdentityResource(
                    "subscriptionlevel",
                    "Your subscription level",
                    new List<string>() { "subscriptionlevel" })
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[] {
                new ApiResource(
                    "imagegalleryapi",
                    "Image Gallery API",
                    new[] { "role" })
                {
                    Scopes = { "imagegalleryapi"},
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("defaultScope", "default API"), //this is a test scope
                new ApiScope("imagegalleryapi", "Image Gallery API"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 120,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    // AllowedGrantTypes = GrantTypes.CodeAndClientCredentials, //for some reason web-client doesn't work with this
                    AllowedGrantTypes = GrantTypes.Code, //client_credentials doesn't work as this is a authorization code flow
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = new List<string>(){
                        "https://localhost:44389/signin-oidc"
                        },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "defaultScope",
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "imagegalleryapi",
                        "country",
                        "subscriptionlevel"
                    },
                    RequirePkce = true,
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44389/signout-callback-oidc"
                    }
                } 
            };
    }
}