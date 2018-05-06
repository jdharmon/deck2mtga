using Deck2MTGA.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Deck2MTGA.Tests
{
    public class CardTests
    {
        private Card _card;

        public CardTests()
        {
            _card = new Card
            {
                Name = "Swamp",
                Set = "DOM",
                CollectorNumber = 258
            };
        }

        [Fact]
        public void ToDeckString()
        {
            var text = _card.ToString();
            var expression = new Regex(@"^\d+ [\w ]+$");

            Assert.Matches(expression, text);
        }

        [Fact]
        public void ToArenaString()
        {
            var text = _card.ToArenaString();
            var expression = new Regex(@"^\d+ [\w ]+ \(\w{3}\) \d+$", RegexOptions.Multiline);

            Assert.Matches(expression, text);
        }

        [Fact]
        public void Convert_DOMToDAR()
        {
            var text = _card.ToArenaString();

            Assert.Contains("(DAR)", text);
        }
    }
}
