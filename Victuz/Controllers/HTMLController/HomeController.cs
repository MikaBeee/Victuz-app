﻿using Microsoft.AspNetCore.Mvc;
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
                .Where(g => g.IsSuggested == false)
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
        public async Task<IActionResult> Dashboard()
        {
            var newSuggestionsCount = await _context.gathering
                .Where(g => g.IsSuggested == true)
                .Where(g => g.Votes.Count > 3)
                .CountAsync();

            if (newSuggestionsCount > 0)
            {
                ViewData["NewSuggestions"] = $"Er is een nieuwe suggestie voor een activiteit! ({newSuggestionsCount} nieuwe suggesties)";
            }

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult TicketScanner() 
        {
            return View();
        }

        //GET ticket by unique qr code
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetTicketDetailsbyUniqueCode(string ticketGuid)
        {
            // Decode the ticketGuid to extract GatheringId, UserId, and RegistrationDate
            string[] parts = ticketGuid.Split('-');
            if (parts.Length != 3)
            {
                return BadRequest("Invalid ticket GUID format.");
            }

            int gatheringId = int.Parse(parts[0]);
            int userId = int.Parse(parts[1]);


            // Retrieve the ticket based on these values
            var ticket = _context.gatheringRegistration
                .Include(gr => gr.User)  
                .Include(gr => gr.Gathering) 
                .Where(gr => gr.GatheringId == gatheringId && gr.UserId == userId)
                .Select(gr => new
                {
                    UserName = gr.User.UserName,
                    GatheringTitle = gr.Gathering.GatheringTitle,
                    RegistrationDate = gr.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    UniqueCode = $"{gr.GatheringId}-{gr.UserId}-{gr.RegistrationDate:MM/dd/yyyy HH:mm:ss}",
                    TicketImageUrl = Url.Action("GetTicketImage", "GatheringRegistrations", new { guid = $"{gr.GatheringId}-{gr.UserId}-{gr.RegistrationDate:MM/dd/yyyy HH:mm:ss}" })
                })
                .FirstOrDefault();

            if (ticket == null)
            {
                return NotFound(); 
            }

            return Ok(ticket); 
        }
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public IActionResult DeleteTicket(string ticketGuid)
        {

            string[] parts = ticketGuid.Split('-');
            if (parts.Length != 3)
            {
                return BadRequest("Invalid ticket GUID format.");
            }

            if (!int.TryParse(parts[0], out int gatheringId) || !int.TryParse(parts[1], out int userId))
            {
                return BadRequest("Invalid ticket GUID format.");
            }


            if (!DateTime.TryParseExact(parts[2], "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime registrationDate))
            {
                return BadRequest("Invalid registration date format.");
            }


            var ticket = _context.gatheringRegistration
                .FirstOrDefault(gr => gr.GatheringId == gatheringId && gr.UserId == userId);

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

 
            _context.gatheringRegistration.Remove(ticket);
            _context.SaveChanges();

            return Ok("Ticket deleted successfully."); 
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
