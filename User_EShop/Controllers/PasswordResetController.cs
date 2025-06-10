using AdditionalTools;
using AdditionalTools.EmailSender;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.PasswordHasher;
using User.Domain.Repositories;

namespace User_EShop.Controllers
{
    [Route("[controller]")]
    public class PasswordResetController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository _repository;
        private readonly IHasher _hasher;
        private readonly ApplicationDbContext _context;
        public PasswordResetController(IEmailSender emailSender, IUserRepository repository, IHasher hasher, ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _repository = repository;
            _hasher = hasher;
            _context = context;
        }

        [HttpGet]
        [Route("ResetPasswordView")]
        public IActionResult ResetPasswordView()
        {
            return View();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPasswordView", model);
            }

            string code = SixDigitCodeGenerator.GenerateSixDigitCode();
            await _emailSender.SendEmailAsync(model.Email, "Password resset", $"There is code to confirm your email: {code}");
            HttpContext.Session.SetString("code", code);
            HttpContext.Session.SetString("email", model.Email);
            return RedirectToAction("ConfirmPasswordResetView");
        }

        [HttpGet]
        [Route("ConfirmPasswordResetView")]
        public IActionResult ConfirmPasswordResetView()
        {
            return View();
        }

        [HttpPost]
        [Route("ConfirmPasswordReset")]
        public IActionResult ConfirmPasswordReset(RegistrationConfirmation model)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmPasswordResetView", model);
            }

            if (model.ConfirmationCode.Equals(HttpContext.Session.GetString("code")))
            {
                HttpContext.Session.Remove("code");
                return RedirectToAction("SetNewPasswordView");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You entered incorrect password");
                return View("ConfirmPasswordResetView", model);
            }
        }

        [HttpGet]
        [Route("SetNewPasswordView")]
        public IActionResult SetNewPasswordView()
        {
            return View();
        }

        [HttpPost]
        [Route("SetNewPassword")]
        public async Task<IActionResult> SetNewPassword(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SetNewPasswordView", model);
            }

            string? email = HttpContext.Session.GetString("email");
            ArgumentNullException.ThrowIfNull(email);
            UserDb? user = await _repository.GetUserByNameOrEmailAsync(email);
            ArgumentNullException.ThrowIfNull(user);
            user.PasswordHash = _hasher.Hash(model.Password);
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(email, "Password reset", "You succefully reseted your password, don`t forget it again!:)");
            return RedirectToAction("LoginView", "Login");
        }

    }
}
