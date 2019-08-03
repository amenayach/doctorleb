using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using atDoctor.Models.AccountViewModels;
using Health.Identity.Models;
using Health.Identity;
using Health.Doctors;
using Health.Configuration.Sms;

namespace atDoctor.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private const int ActivationExpiryInMinutes = 5;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DoctorManager _doctorManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILogger<AccountController> logger,
            DoctorManager doctorManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = logger;
            _doctorManager = doctorManager;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _userManager.FindByNameAsync(User.Identity.Name));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                    var userActivation = new UserActivation
                    {
                        Id = Guid.NewGuid().ToString(),
                        Created = DateTime.Now,
                        Expires = DateTime.Now.AddMinutes(ActivationExpiryInMinutes),
                        IdentityCode = code,
                        UserId = user.Id,
                        ShortCode = Health.Configuration.Extensions.GetRandomNumber()
                    };

                    await _doctorManager.AddUserActivation(userActivation);

                    //Send SMS with the short code
                    _smsSender.Send(model.Mobile, "Activation code: " + userActivation.ShortCode);

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(model.Email))
                        {
                            await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                        }
                    }
                    catch // Ignored
                    {
                    }

                    _logger.LogInformation("User created a new account with password.");
                    return Ok(new { Success = true });
                }
                AddErrors(result);
            }

            return BadRequest(new { Success = false, Msg = "Invalid model" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReSendCode(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(nameof(username));
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("Invalid username");
            }

            var userActivation = await _doctorManager.GetUserActivation(user.Id, string.Empty);

            if (userActivation == null)
            {
                return BadRequest("Invalid username info");
            }

            if (userActivation.Expires > DateTime.Now)
            {
                return BadRequest("Please wait until the SMS is delivered");
            }

            if ((DateTime.Now - userActivation.Expires).TotalMinutes > 2 * ActivationExpiryInMinutes)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Please check your email");
                }

                return BadRequest("Please register with your email");
            }

            _smsSender.Send(user.Mobile, "Activation code: " + userActivation.ShortCode);

            userActivation.Expires = DateTime.Now.AddMinutes(-3 * ActivationExpiryInMinutes);

            await _doctorManager.UpdateUserActivation(userActivation);

            return Ok(new { Success = true });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{username}/{code}")]
        public async Task<IActionResult> ConfirmEmail(string username, string code)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(code))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with name '{username}'.");
            }

            if (code.Length < 10)
            {
                var activation = await _doctorManager.GetUserActivation(user.Id, code);

                if (activation != null)
                {
                    code = activation.IdentityCode;
                }
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Json(new { Success = result.Succeeded ? "ConfirmEmail" : "Error" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(model.Email);
                }

                if (user == null)
                {
                    return BadRequest(new { Success = false, Msg = "User not found" });
                }

                if (user != null && !user.EmailConfirmed)
                {
                    return BadRequest(new { Success = false, Msg = "Email not confirmed" });
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Ok(user);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return BadRequest(new { Success = false, Msg = "Locked out!!!" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(new { Success = false, Msg = "Invalid login attempt." });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest(new { Success = false });
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
            return BadRequest(new { Success = true });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest();
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return BadRequest();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }

        #endregion
    }
}
