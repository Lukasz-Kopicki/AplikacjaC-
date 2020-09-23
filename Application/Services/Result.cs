using Application.ViewModels;

namespace Application.Services
{
    public class Result
    {
        public enum ResultState { Invalid, Succeeded, Cancelled, Interrupted, Failed, Pending } 
        public ResultState StateResult { get; set; }
        public string ErrorMessage { get; set; }
        public LoginViewModel LoginModel { get; set; }
        public RegisterViewModel RegisterModel { get; set; }
        public string ReturnUrl { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}