using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deck2MTGA.Tests
{
    public class CardRepositoryTests
    {
        const string CARD_NAME = "Opt";

        private readonly Mock<IMtgDbContext> _dbContext;
        private readonly CardRepository _repository;

        public CardRepositoryTests()
        {
            var cards = new List<Card>()
            {
                new Card
                {
                    MultiverseId = 22988,
                    Name = CARD_NAME,
                    Set = "INV",
                    Number = "65"
                },
                new Card
                {
                    MultiverseId = 435217,
                    Name = CARD_NAME,
                    Set = "XLN",
                    Number = "65"
                },
                new Card
                {
                    MultiverseId = 442948,
                    Name = CARD_NAME,
                    Set = "DOM",
                    Number = "60"
                }
            }.AsQueryable();

            _dbContext = new Mock<IMtgDbContext>();
            _dbContext.As<IMtgDbContext>().Setup(m => m.Cards).Returns(cards);

            Environment.SetEnvironmentVariable("LEGAL_SETS", "AKH;HOU;XLN;RIX");
            _repository = new CardRepository(_dbContext.Object);
        }

        [Fact]
        public void Find_Found()
        {
            var card = _repository.Find(CARD_NAME);

            Assert.NotNull(card);
            Assert.Equal("Opt", card.Name);
            Assert.Equal("XLN", card.Set);
        }

        [Fact]
        public void Find_NotFound()
        {
            var card = _repository.Find("foo");

            Assert.Null(card);
        }
    }
}
