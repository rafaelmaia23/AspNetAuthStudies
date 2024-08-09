using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

const string AuthScheme = "cookie";
const string AuthScheme2 = "cookie2";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(AuthScheme)
    .AddCookie(AuthScheme)
    .AddCookie(AuthScheme2);

builder.Services.AddAuthorization(builder => 
{
    builder.AddPolicy("eu passport", pb =>
    {
        pb.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthScheme)
            .AddRequirements()
            .RequireClaim("passport_type", "eur");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// app.Use((ctx, next) => 
// {
//     if (ctx.Request.Path.StartsWithSegments("/login"))
//     {
//         return next();
//     }

//     if(!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
//     {
//         ctx.Response.StatusCode = 401;
//         return Task.CompletedTask;
//     }

//     if (!ctx.User.HasClaim("passport_type", "eur"))
//     {
//         ctx.Response.StatusCode = 403;
//         return Task.CompletedTask;
//     }

//     return next();
// });

//if using controllers
// [Authorize(Policy = "eu passport")]
app.MapGet("/unsecure", (HttpContext ctx) => 
{    
    return ctx.User.FindFirst("urs")?.Value ?? "empty";
});

app.MapGet("/sweden", (HttpContext ctx) => 
{
    // if(!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
    // {
    //     ctx.Response.StatusCode = 401;
    //     return "";
    // }

    // if (!ctx.User.HasClaim("passport_type", "eur"))
    // {
    //     ctx.Response.StatusCode = 403;
    //     return "";
    // }

    return "allowed";
}).RequireAuthorization("eu passport");;

app.MapGet("/norway", (HttpContext ctx) => 
{
    // if(!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
    // {
    //     ctx.Response.StatusCode = 401;
    //     return "";
    // }

    // if (!ctx.User.HasClaim("passport_type", "nor"))
    // {
    //     ctx.Response.StatusCode = 403;
    //     return "";
    // }

    return "allowed";
});

app.MapGet("/denmark", (HttpContext ctx) => 
{
    // if(!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme2))
    // {
    //     ctx.Response.StatusCode = 401;
    //     return "";
    // }

    // if (!ctx.User.HasClaim("passport_type", "eur"))
    // {
    //     ctx.Response.StatusCode = 403;
    //     return "";
    // }

    return "allowed";
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "anton"));
    claims.Add(new Claim("passport_type", "eur"));
    var identity = new ClaimsIdentity(claims, AuthScheme);
    var user = new ClaimsPrincipal(identity);
    await ctx.SignInAsync(AuthScheme ,user);
}).AllowAnonymous();

app.Run();

public class MyRequirement : IAuthorizationRequirement {}

public class MyRequirementHandler : AuthorizationHandler<MyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirement requirement)
    {
        //context.User
        // context.Succeed(new MyRequirement());
        return Task.CompletedTask;
    }
}