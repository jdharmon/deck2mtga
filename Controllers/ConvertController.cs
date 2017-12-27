using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Deck2MTGA_Web
{
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        // POST api/convert
        [HttpPost]
        public string Post([FromBody]string input)
        {
            var deck = Deck.Parse(input);
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
