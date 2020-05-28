using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RouterLab.Models;

namespace RouterLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Films()
        {
            return RedirectToAction("Index", "Films");
        }
        public IActionResult Routers()
        {
            return RedirectToAction("Index", "Routers");
        }
        public IActionResult Standarts()
        {
            return RedirectToAction("Index", "Standarts");
        }

        public IActionResult Speeds()
        {
            return RedirectToAction("Index", "Speeds");
        }

        public IActionResult Diapasons()
        {
            return RedirectToAction("Index", "Diapasons");
        }
        public IActionResult RouterStandarts()
        {
            return RedirectToAction("Index", "RouterStandarts");
        }

        public IActionResult Prices()
        {
            return RedirectToAction("Index", "Prices");
            
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
