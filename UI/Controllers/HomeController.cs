using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Business.Servicess;
using System.Diagnostics;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IMailService mailService, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _mailService = mailService;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var codeToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackURL = Url.Action("ConfirmEmail", "Home", new
                    {
                        userId = user.Id,
                        code =
                        codeToken
                    }, protocol: Request.Scheme);
                    var mailMessage = $"Please confirm your account by <br/> <a href = '{callbackURL}'> clicking here</a>";
                    await _mailService.SendMailAsync(model.Email, "Confirm your email", mailMessage);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Kullan�c� ge�ersiz");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "Index" : "Error");
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginVM loginVM)
        {
            return View();
        }
       /* public IActionResult SendTestEmail()
        {
            _mailService.SendMail("recipient@example.com", "Test Email", "This is a email.");
            return View();
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
