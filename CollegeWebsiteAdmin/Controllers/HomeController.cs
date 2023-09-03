using CollegeWebsiteAdmin.Extensions;
using CollegeWebsiteAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text.Json;

namespace CollegeWebsiteAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;
        public HomeController(ILogger<HomeController> logger, MyDBContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Teacher ValidUser = _context.Teachers
                .Where(x => x.TeacherName == username && x.Telephone == password)
                .FirstOrDefault();

            if (ValidUser != null)
            {
                HttpContext.Session.Set("LoggedInUser", ValidUser);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}