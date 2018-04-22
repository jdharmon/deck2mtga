using Deck2MTGA.Web;
using Deck2MTGA.Web.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Deck2MTGA.Tests
{
    public class DeckTests
    {
        private readonly Mock<ICardRepository> _repository;
        private readonly Deck _deck;

        public DeckTests()
        {
            _repository = new Mock<ICardRepository>();
            _deck = new Deck(_repository.Object);

            _repository.Setup(m => m.Find(It.IsAny<string>()))
                .Returns((string name) => new Card
                {
                    Name = name,
                    Set = "XXX",
                    CollectorNumber = name.GetHashCode()
                });
        }

        [Fact]
        public void Parse_Valid()
        {
            _deck.Parse("36 Relentless Rats\n24 Swamp");

            Assert.NotEmpty(_deck.Cards);
            Assert.Empty(_deck.Errors);
        }

        [Fact]
        public void Parse_CardNotFound()
        {
            _repository.Setup(m => m.Find(It.IsAny<string>()))
                .Throws(new DataException());

            _deck.Parse("60 foo");

            Assert.Empty(_deck.Cards);
            Assert.NotEmpty(_deck.Errors);
        }

        [Fact]
        public void Parse_InvalidFormat()
        {
            _deck.Parse("foo");

            Assert.Empty(_deck.Cards);
            Assert.NotEmpty(_deck.Errors);
        }

        [Fact]
        public void Parse_Blank()
        {
            _deck.Parse(null);

            Assert.Empty(_deck.Cards);
            Assert.Empty(_deck.Errors);
        }

        [Fact]
        public void ToDeckString()
        {
            _deck.Parse("36 Relentless Rats\n24 Swamp");

            var text = _deck.ToDeckString();
            var expression = new Regex(@"^\d+ [\w ]+\r?\n", RegexOptions.Multiline);

            Assert.Matches(expression, text);
        }

        [Fact]
        public void ToArenaString()
        {
            _deck.Parse("36 Relentless Rats\n24 Swamp");

            var text = _deck.ToArenaString();
            var expression = new Regex(@"^\d+ [\w ]+ \(\w{3}\) \d+\r?\n", RegexOptions.Multiline);

            Assert.Matches(expression, text);
        }
    }
}
