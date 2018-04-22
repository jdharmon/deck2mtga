using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Deck2MTGA.Web
{
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        private readonly ICardRepository _cardRepository;

        public ConvertController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        // POST api/convert
        [HttpPost]
        public string Post([FromBody]string input)
        {
            var deck = new Deck(_cardRepository);
            deck.Parse(input);
            return deck.ToArenaString();
        }

        [HttpPost("Raw")]
        public string PostRaw() 
        {
            string input;
            using (var reader = new StreamReader(Request.Body)) {
                input = reader.ReadToEnd();
            }
            return Post(input);
        }
    }
}
