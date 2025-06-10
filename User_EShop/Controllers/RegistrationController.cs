using AdditionalTools.EmailSender;
using AutoMapper;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.PasswordHasher;
using System.Text.Json;
using System.Text;
using User.Domain.Repositories;
using AdditionalTools;

namespace User_EShop.Controllers
{
    [Route("[controller]")]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService _registration;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IEmailSender _email;
        private readonly IHasher _hasher;
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _repository;

        public RegistrationController(IRegistrationService registration, IMapper mapper,
            ILogger<RegistrationController> logger, IEmailSender email,
            IHasher hasher, ApplicationDbContext context, IUserRepository repository)
        {
            _registration = registration;
            _mapper = mapper;
            _logger = logger;
            _email = email;
            _hasher = hasher;
            _context = context;
            _repository = repository;
        }

        [HttpGet]
        [Route("RegistrationView")]
        public ViewResult RegistrationView()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegistrationViewModel view_model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{ModelState}");
                return View("RegistrationView", view_model);
            }
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                view_model.Password = _hasher.Hash(view_model.Password);
                var user = _mapper.Map<UserDb>(view_model);
                await _registration.AddNewUserToDatabaseAsync(user);
                HttpContext.Session.SetString("User", user.Id.ToString());

                string confirmation = SixDigitCodeGenerator.GenerateSixDigitCode();

                await _email.SendEmailAsync(user.Email, "Registration confirmation",
                    $"To confirm your registration, please enter this code {confirmation}");

                HttpContext.Session.SetString("ConfirmationCode", confirmation);

                await transaction.CommitAsync();

                _logger.LogInformation("New user {Username} was added to database {UserDb}", user.Name, nameof(UserDb));
                return RedirectToAction("ConfirmRegistrationView");
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning("Failed to authorize to email in MailKit");
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConfirmRegistrationView")]
        public IActionResult ConfirmRegistrationView()
        {
            return View();
        }

        [HttpPost("ConfirmRegistration")]
        public async Task<IActionResult> ConfirmRegistration(RegistrationConfirmation registration)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRegistrationView", registration);
            }
            string? password = HttpContext.Session.GetString("ConfirmationCode");
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Confirmation code didn`t saved in session appropriately!");
            }
            if (registration.ConfirmationCode.Equals(password))
            {
                HttpContext.Session.Remove("ConfirmationCode");
                int user_id = Convert.ToInt32(HttpContext.Session.GetString("User"));
                UserDb user = await _repository.GetUserByIdAsync(user_id);
                user.IsEmailConfirmed = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("CompleteRegistration");
            }
            else return BadRequest("Your code is incorrect!");
        }

        [HttpGet("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration()
        {
            
            int user_id = Convert.ToInt32(HttpContext.Session.GetString("User"));
            UserDb? user = await _repository.GetUserByIdAsync(user_id);
            HttpContext.Session.Remove("User");
            await _email.SendEmailAsync(user.Email, "Succefull registration", "Congratulations, you succefully registered!\r\nWelcome to EShop_Cars!");
            return RedirectToAction("LoginView", "Login");
        }
    }
}
