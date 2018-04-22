using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;

namespace Deck2MTGA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICardRepository _cardRepository;

        public HomeController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult ConvertDeck([FromForm]string input)
        {
            var deck = new Deck(_cardRepository);
            deck.Parse(input);
            return View("Index", deck);
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
