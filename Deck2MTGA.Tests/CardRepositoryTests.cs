using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
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
                    Name = CARD_NAME,
                    Set = "xln",
                    Number = "65"
                }
            }.AsQueryable();

            _dbContext = new Mock<IMtgDbContext>();
            _dbContext.As<IMtgDbContext>().Setup(m => m.Cards).Returns(cards);

            _repository = new CardRepository(_dbContext.Object);
        }

        [Fact]
        public void Find_Found()
        {
            var card = _repository.Find(CARD_NAME);

            Assert.NotNull(card);
            Assert.Equal("Opt", card.Name);
        }

        [Fact]
        public void Find_NotFound()
        {
            var card = _repository.Find("foo");

            Assert.Null(card);
        }
    }
}
