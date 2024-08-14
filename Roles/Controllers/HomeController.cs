using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Roles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public string Index() => "Index Route";

        [HttpGet("/secret")]
        [Authorize(Roles = "admin")]
        public string Secret() => "Secret Route";
    }
}
