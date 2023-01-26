﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RssSubscriptionManagement.Interfaces;
using RssSubscriptionManagement.Services;

namespace RssSubscriptionManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityProvider _identityprovider;
        public AccountController(SignInManager<IdentityUser> signInManager, IIdentityProvider identityProvider)
        {
            _signInManager = signInManager;
            _identityprovider = identityProvider;
        }
        [HttpGet, Route("register")]
        public async Task<IActionResult> Test()
        {
            return Content("Test");
        }
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var dictionary = await _identityprovider.CreateUserAsync(Email, Password);

                if (dictionary.ElementAt(0).Key.Succeeded)
                {
                    await _signInManager.SignInAsync(dictionary.ElementAt(0).Value, false);
                    return Content("Registred");
                }
                else
                {
                    foreach (var error in dictionary.ElementAt(0).Key.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Content("SomeError");
        }
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(Email, Password, false, false);
                if (result.Succeeded)
                {
                    return Content("Done");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Login or Password");
                }
            }
            return Content("Error");
        }
    }
}
