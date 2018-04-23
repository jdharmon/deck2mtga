using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]string input)
        {
            if (input == null)
                return BadRequest();

            var deck = new Deck(_cardRepository);
            deck.Parse(input);
            var result = deck.ToArenaString();

            if (Request.Headers["Accept"] == "text/plain")
                return Ok(result);
            else
                return Ok(result.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
        }

        [HttpPost("Raw")]
        [Consumes("text/plain")]
        [Produces("text/plain")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(string))]
        public IActionResult PostRaw() 
        {
            string input;
            using (var reader = new StreamReader(Request.Body)) {
                input = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(input))
                return BadRequest();

            var deck = new Deck(_cardRepository);
            deck.Parse(input);
            var result = deck.ToArenaString();

            return Ok(result);
        }
    }
}
