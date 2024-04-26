using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SavorySeasons.Models;
using SavorySeasons.Services.Jwt;
using System.Security.Claims;
using System.Net;
using System;
using System.Data;

namespace SavorySeasons.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _tokenService;
        private readonly JwtConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService tokenService,
            JwtConfiguration configuration)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<ActionResult<AdminLoginDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail) ?? await _userManager.FindByUserName(loginDto.UserNameOrEmail);
            if (user == null)
                return BadRequest("UserName Or Email  not found");

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return BadRequest("Password does not match");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var refreshToken = _tokenService.CreateRefreshToken();
            await _userManager.UpdateAsync(user);

            return new AdminLoginDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(claims, 24),
                RefreshToken = refreshToken,
                UserName = user.UserName,
                Role = roles.FirstOrDefault().ToString()
            };
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser
                {
                    Name = model.Name,
                    PhoneNumber = model.Mobile,
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Something went wrong");
                }

                await _userManager.AddToRoleAsync(user, ApplicationUserRoles.User);

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
