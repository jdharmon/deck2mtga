using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deck2MTGA_Web.Models;

namespace Deck2MTGA_Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult ConvertDeck([FromForm]string input)
        {
            return View("Index", Deck.Parse(input));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Text to Magic Arena Import";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
