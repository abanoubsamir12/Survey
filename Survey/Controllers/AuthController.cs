using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Tls;
using Survey.Data;
using Survey.DTOs;
using Survey.Models;

namespace Survey.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SurveyContext _context;

        public AuthController(IConfiguration configuration, SurveyContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Email,Role,Password")] CreateUserDto user)
        {

            if (ModelState.IsValid)
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);


                try
                {
                    User newUser = new User(user.Username, user.Email,user.Role, PasswordHash);


                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Successful registration!";

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        var message = ex.InnerException.Message;

                        if (message.Contains("IX_Users_Username"))
                        {
                            ModelState.AddModelError("Username", "This username is already taken.");
                        }
                        else if (message.Contains("IX_Users_Email"))
                        {
                            ModelState.AddModelError("Email", "This email is already registered.");
                        }
                    }

                    return View(user); 

                }

            }
            else
            {
                return View(user);
            }

            return RedirectToAction("Index", "Home"); 
        }




        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.password))
            {
                return Unauthorized("Invalid credentials.");
            }
            if(user.Role == Role.Admin && !user.isActive)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddMinutes(60),
                SameSite = SameSiteMode.Unspecified
            });
            if(user.Role != Role.Admin)
                return RedirectToAction("Index", "UserHome");
            else return RedirectToAction("Index", "Admin");
        }

        private string GenerateJwtToken(User user)
        {
            // Define the claims included in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),               
                new Claim(JwtRegisteredClaimNames.Email, user.Email),                // Email claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   // Unique token identifier
                new Claim("UserId", user.Id.ToString())                              // Custom UserId claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],               
                audience: _configuration["Jwt:Audience"],           
                claims: claims,                                      
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])), 
                signingCredentials: creds                           
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
