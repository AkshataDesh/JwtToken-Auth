using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Demo.Data;
using WebAPI_Demo.Entities;
using WebAPI_Demo.Models;

namespace WebAPI_Demo.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly MyContext _context;
      
        public AuthService(IConfiguration config, MyContext context)
        {
            _config = config;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<User> RegisterAsync(UserDTO userrequest)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == userrequest.UserName)) return null;

            User u = new User();
            u.UserName = userrequest.UserName;
            u.PasswordHash = new PasswordHasher<User>().HashPassword(u, userrequest.Password);
            _context.Users.Add(u);
            _context.SaveChanges();
            return u;

        }
       
        public async Task<string> LoginAsync(UserDTO userrequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userrequest.UserName);  
            if (userrequest.UserName != user.UserName) return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userrequest.Password) == PasswordVerificationResult.Failed)
                return null;

            string token = CreateToken(user);

            return token;

        }

        public string GenerateAndSaveRefereshToken(User u)
        {
            int a=10;
            var c=10+a;
            return "xyz";
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("token:key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<string>("token:issuer"),
                audience: _config.GetValue<string>("token:audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }

}
