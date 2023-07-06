using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        // GET: api/<RolesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = Roles.All;

            return Ok(result);
        }

        // GET api/<RolesController>/5
       
    }
}
