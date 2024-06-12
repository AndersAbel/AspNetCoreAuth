using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Mvc;

public static class Helpers
{
    public const string Thumbprint = "b7da225ae3ed72956e40e1777a63db114bdc545a";

    public static void ConfigureIdentityServer(this OpenIdConnectOptions opt)
    {
        opt.Authority = "https://demo.duendesoftware.com";
        opt.ClientId = "interactive.confidential";
        opt.ClientSecret = "secret";
        opt.ResponseType = "code";

        opt.SaveTokens = true;

        opt.MapInboundClaims = false;
        opt.GetClaimsFromUserInfoEndpoint = true;

        opt.TokenValidationParameters.NameClaimType = "name";
        opt.TokenValidationParameters.RoleClaimType = "role";
    }
}
