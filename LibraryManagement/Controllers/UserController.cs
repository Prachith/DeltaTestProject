using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var newUser = await _userService.AddUser(user);

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {            
            var login = await _userService.Login(user);
            if (login.Email == null)
            {
                ViewBag.message = "User Login Details Failed!!";
                return View();
            }
            
            HttpContext.Session.SetString(SessionVariables.SessionKeyEmail, login.Email);
            HttpContext.Session.SetString(SessionVariables.SessionKeyUserId, login.UserId);
            HttpContext.Session.SetInt32(SessionVariables.SessionKeyUserIdKey, login.Id);
            return RedirectToAction("Index", "Home");
            
        }               
    }
}
