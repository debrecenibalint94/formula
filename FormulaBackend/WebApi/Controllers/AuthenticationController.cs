using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.DataAccess.Models;
using WebApi.Api.ViewModels;
using WebApi.Services;

namespace WebApi.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException("userManager");
            }

            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _configuration = configuration;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationViewModel>> Authenticate(UserViewModel userViewModel)
        {
            var user = await _userService.GetUserByUserNameAndPassword(userViewModel.Username, userViewModel.Password);
            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["SecurityKey"]);

                int expiresIn;
                if (!int.TryParse(_configuration["TokenExpires"], out expiresIn))
                {
                    expiresIn = 1800;
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userViewModel.Username)
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(expiresIn),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var createdToken = tokenHandler.WriteToken(token);

                return new AuthenticationViewModel
                {
                    Token = createdToken,
                    ExpiresIn = expiresIn
                };

            }
            else
            {
                ModelState.AddModelError("Username", "Username or Password is not correct.");
                ModelState.AddModelError("Password", "Username or Password is not correct.");
                return ValidationProblem(ModelState);
            }
        }
    }
}