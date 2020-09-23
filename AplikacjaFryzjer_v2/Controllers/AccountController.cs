using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels;
using DataAccessLogic.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
//test
namespace AplikacjaFryzjer_v2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;

        public AccountController(
            IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = await _userManager.Login(returnUrl);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Result result = await _userManager.Login(model);
            if (result.StateResult == Result.ResultState.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("AccesDenied");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Result result = new Result();
            if (ModelState.IsValid)
            {
                result = await _userManager.Register(model, User);
                return RedirectToAction(result.Action, result.Controller);
            }
            return View(result.RegisterModel);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            if (await _userManager.VerifyEmail(userId, code) == true)
            {
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            if (await _userManager.LogOut() == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var challengeResult = _userManager.ExternalLogin(provider, returnUrl);
            return challengeResult;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            Result result = await _userManager.ExternalLoginCallback(returnUrl, remoteError);

            if (result.StateResult == Result.ResultState.Failed)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View("Login", result.ErrorMessage);
            }
            else if (result.StateResult == Result.ResultState.Interrupted)
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View("Error");
            }
            else
            {
                return LocalRedirect(result.ReturnUrl);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
