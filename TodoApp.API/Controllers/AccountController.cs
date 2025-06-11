using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data.DataModels.Dto.UserDtos;
using TodoApp.Data.DataModels.Entities;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(SignInManager<UserEntity> signInManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            UserEntity user = mapper.Map<UserEntity>(registerDto);
            user.UserName = registerDto.Email;

            IdentityResult result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem(ModelState);
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            UserEntity? user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            Microsoft.AspNetCore.Identity.SignInResult? results = 
                await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!results.Succeeded)
            {
                return Unauthorized();
            }
            await signInManager.SignInAsync(user, isPersistent: false);

            //var authCookie = Response.Headers.FirstOrDefault(header => header.Key.Contains("Set-Cookie"));
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
