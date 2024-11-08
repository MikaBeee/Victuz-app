using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Victuz.Models;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Victuz.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Http;


namespace Victuz.Controllers.HTMLController
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly VictuzDB _context;

        public HomeController(ILogger<HomeController> logger, VictuzDB context)
        {
            _logger = logger;
            _context = context;
            
        }

        public IActionResult Index()
        {
            var model = new OrderViewModel
            {
                gatherings = _context.gathering
                .Include(g => g.Location)
                .OrderBy(g => g.Date)
                .ToList() ?? new List<Gathering>(),
                forums = _context.forum.ToList() ?? new List<Forum>()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Activities()
        {
            return View();
        }

        public IActionResult Forum()
        {
            var forums = _context.forum.ToList();
            return View(forums);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            var statusCode = HttpContext.Response.StatusCode;
            string statusDescription = "An error occurred.";

            if (statusCode == 404)
            {
                return new ObjectResult(new { message = "Page not found" })
                {
                    StatusCode = statusCode
                };
            }

            // Default error view with status code
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode,  // Pass status code to view
                StatusDescription = statusDescription // You can pass the description if needed in the view
            });
        }
    }
}
