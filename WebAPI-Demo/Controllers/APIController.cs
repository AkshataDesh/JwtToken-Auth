using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Demo.Entities;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services;

namespace WebAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly AuthService _service;  
        public APIController(AuthService service)
        {
            _service=service;
        }

        [HttpPost("register")]
        public async  Task<ActionResult<User>> Register(UserDTO user)
        {
            var u= await _service.RegisterAsync(user);
            if(u== null) return BadRequest("UserName already exists");  

            return Ok(u);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO user)
        {

           var token=await _service.LoginAsync(user);
            if (token == null) return NotFound("user and password not found");
            return Ok(token);   
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("API is working after authentication");
        }

        [HttpGet("admin")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetAdmin()
        {
            return Ok("admin role working for endpoint");
        }

    }
      
}
