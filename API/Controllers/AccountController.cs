using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseAPIController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {
            if (await EmailExists(registerDTO.Email))
            {
                return BadRequest("Email id exists !");
            }

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.ToDto(tokenService); // Extension method
        }

        private async Task<bool> EmailExists(string Email)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower() == Email.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LogInDTO logInDTO)
        {
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email == logInDTO.Email);
            if (user == null) return Unauthorized("Invalid Email address !");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logInDTO.Password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password !");
            }

            return user.ToDto(tokenService); // Extension method
        }

    }
}
