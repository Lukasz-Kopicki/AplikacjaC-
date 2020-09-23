using Application.Interfaces;
using Application.ViewModels;
using DataAccessLogic.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUrlHelper _urlHelper;
        private readonly IEmailService _emailService;
        private readonly IActionContextAccessor _actionContext;

        public UserManagerService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IUrlHelperFactory urlHelper,
            IEmailService emailService,
            IActionContextAccessor actionContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelper = urlHelper.GetUrlHelper(actionContext.ActionContext);
            _emailService = emailService;
            _actionContext = actionContext;
        }

        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = _urlHelper.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<LoginViewModel> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return model;
        }

        public async Task<Result> Login(LoginViewModel model)
        {
            Result result = new Result();
            //login functionality
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (signInResult.Succeeded)
                {
                    result.StateResult = Result.ResultState.Succeeded;
                    return result;
                }

            }
            result.StateResult = Result.ResultState.Failed;
            return result;
        }

        public async Task<bool> LogOut()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<Result> Register(RegisterViewModel model, ClaimsPrincipal claims)
        {
            Result result = new Result();
            //register functionality
            var newUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };
            

            var resultIdentity = await _userManager.CreateAsync(newUser, model.Password);
           

            if (resultIdentity.Succeeded)
            {
                if (_signInManager.IsSignedIn(claims) && claims.IsInRole("Admin"))
                {
                    result.StateResult = Result.ResultState.Succeeded;
                    result.Action = "ListUsers";
                    result.Controller = "Administrator";
                    return result;
                }
                //login user
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                //generation of the email token
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                //var link = _urlHelper.Action(nameof(VerifyEmail), "Home", new { userId = newUser.Id, code });

                //await _emailService.SendAsync(newUser.Email, "Weryfikacja adresu e-mail", $"<a href=\"{link}\">Potwierdź e-mail</a>", true);

                //result.Action = "EmailVerification";
                return result;
            }

            result.Action = "Register";
            result.RegisterModel = model;
            return result;
        }

        public async Task<bool> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<Result> ExternalLoginCallback(string returnUrl, string remoteError)
        {
            Result result = new Result()
            {
                ReturnUrl = returnUrl ?? _urlHelper.Content("~/"),
                LoginModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                }
            };
            
            if (remoteError != null)
            {
                result.ErrorMessage = $"Error from external provider: {remoteError}";
                result.StateResult = Result.ResultState.Failed;
                return result;
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                result.ErrorMessage = $"Error loading external login information.";
                result.StateResult = Result.ResultState.Failed;
                return result;
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                result.StateResult = Result.ResultState.Succeeded;
                return result;
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    result.StateResult = Result.ResultState.Succeeded;
                    return result;
                }

                result.ErrorMessage = $"Nie otrzymano informacji o adresie e-mail od dostawcy: {info.LoginProvider}";
                result.StateResult = Result.ResultState.Interrupted;
                return result;
            }
        }
    }
}
