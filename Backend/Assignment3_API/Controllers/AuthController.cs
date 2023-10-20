using Assignment3_Backend.Models;
using Assignment3_Backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assignment3_Backend.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        { 

            if (string.IsNullOrWhiteSpace(userViewModel.Email) || string.IsNullOrWhiteSpace(userViewModel.Password))
            {
                return BadRequest("Username and password are required.");
            }


            var existingUser = await _userManager.FindByNameAsync(userViewModel.Email);
               if (existingUser != null)
                {
                    return Conflict("Username already exists.");
                }

            var newUser = new AppUser
            {
                UserName = userViewModel.Email,
                Email = userViewModel.Email

            };

             var result = await _userManager.CreateAsync(newUser, userViewModel.Password);
                if (result.Succeeded)
                {
                    return Ok(newUser);
                }
                else
                {
                    // If registration failed, return the errors.
                    return BadRequest(result.Errors);
                }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.Email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userViewModel.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Return a token or some other authentication response if login is successful.
                return Ok("Login successful.");
            }
            else
            {
                return Unauthorized("Invalid login credentials.");
            }
        }
    }
}
