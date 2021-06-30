using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Configuration;
using TodoApi.Models.DTOs.Request;
using TodoApi.Models.DTOs.Response;

namespace TodoApi.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                //We can utillise the model
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() { "Email already in use" },
                        Success = false
                    });
                }

                var newUser = new IdentityUser() { Email = request.Email, UserName = request.Username };
                var isCreated = await _userManager.CreateAsync(newUser, request.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponse()
                    {
                        Success = true,
                        Token = jwtToken,
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false,
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() { "invalid payload" },
                Success = false,
            });
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                IdentityUser existingUser = await _userManager.FindByEmailAsync(request.Email);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }



                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, request.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() { "invalid payload" },
                Success = false,
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandlder = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]{
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandlder.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandlder.WriteToken(token);
            return jwtToken;
        }
    }
}