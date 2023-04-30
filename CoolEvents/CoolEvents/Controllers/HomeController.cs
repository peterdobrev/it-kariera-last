using CoolEvents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CoolEvents.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoolEventsDBContext _context;

        public HomeController(ILogger<HomeController> logger, CoolEventsDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToAction("Login", "Users");
            }

            return _context.Events != null ?
                        View(await _context.Events.ToListAsync()) :
                        Problem("Entity set 'CoolEventsDBContext.Events'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult DataCount()
        {
            if(HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return new JsonResult(new { error = "Not admin, so not showing" })
                {
                    StatusCode = 420
                };
            }
            else
            {
                int eventsNumber = _context.Events.Count();
                int usersNumber = _context.Users.Count();
                int ticketsNumber = _context.Tickets.Count();
                return PartialView("_DataCount", new Tuple<int, int, int>(eventsNumber, usersNumber, ticketsNumber));
            } 
        }
    }
}