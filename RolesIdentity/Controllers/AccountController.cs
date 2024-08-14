using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RolesIdentity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpGet("/login")]
    public async Task<IActionResult> Login(SignInManager<IdentityUser> signInManager)
    {
        await signInManager.PasswordSignInAsync("test@test.com", "password", false, false);

        return Ok();
    }
        
}
