using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserManager
    {
        Task<LoginViewModel> Login(string returnUrl);
        Task<Result> Login(LoginViewModel model);
        Task<Result> Register(RegisterViewModel model, ClaimsPrincipal claims);
        Task<bool> VerifyEmail(string userId, string code);
        Task<bool> LogOut();
        IActionResult ExternalLogin(string provider, string redirectUrl);
        Task<Result> ExternalLoginCallback(string returnUrl = null, string remoteError = null);

    }
}
