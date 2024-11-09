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
using Microsoft.AspNetCore.Authorization;


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
        public IActionResult MeldJeAanPage()
        {
            return View();
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

        [Authorize(Roles = "admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            // Use the provided status code if available, otherwise, use the current response status code
            var code = statusCode ?? HttpContext.Response.StatusCode;
            string statusDescription = "An error occurred.";

            // Customize response for different status codes
            if (code == 404)
            {
                statusDescription = "Page not found.";
            }
            else if (code == 403)
            {
                statusDescription = "Access denied.";
            }
            else if (code == 500)
            {
                statusDescription = "Internal server error.";
            }

            // Set the correct response status code
            HttpContext.Response.StatusCode = code;

            // Optionally, return JSON for specific errors
            if (code == 404)
            {
                return new ObjectResult(new { message = statusDescription })
                {
                    StatusCode = code
                };
            }

            // Default error view with the status code
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = code,
                StatusDescription = statusDescription
            });
        }
    }
}
