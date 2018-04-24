using Deck2MTGA.Web.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Rest;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
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
            _dbContext = new Mock<IMtgDbContext>();
            _repository = new CardRepository(new MemoryCache(new MemoryCacheOptions()), _scryfallClient.Object);



            //Return default card search result
            _scryfallClient.Setup(m => m.Cards.SearchWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<UniqueStrategy?>(), It.IsAny<SortOrder?>(), It.IsAny<SortDirection?>(), It.IsAny<bool?>(), It.IsAny<int?>(), null, default(CancellationToken)))
                .ReturnsAsync(new HttpOperationResponse<CardList>
                {
                    Response = new HttpResponseMessage(HttpStatusCode.OK),
                    Body = new CardList
                    {
                        TotalCards = 1,
                        HasMore = false,
                        Data = new[]
                        {
                            new Card
                            {
                                Name = CARD_NAME,
                                Set = "xln",
                                CollectorNumber = "65"
                            }
                        }
                    }
                });

            _dbContext.Setup(m => m.Cards)
                .Returns(new )
        }

        [Fact]
        public void Find_Found()
        {
            var card = _repository.Find(CARD_NAME);

            Assert.NotNull(card);
            Assert.Equal("Opt", card.Name);
        }

        [Fact]
        public void Find_CacheHit()
        {
            var card1 = _repository.Find(CARD_NAME);
            var card2 = _repository.Find(CARD_NAME.ToLower());
            Assert.Same(card1, card2);
        }

        [Fact]
        public void Find_NotFound()
        {
            //Return NotFound for search
            _scryfallClient.Setup(m => m.Cards.SearchWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<UniqueStrategy?>(), It.IsAny<SortOrder?>(), It.IsAny<SortDirection?>(), It.IsAny<bool?>(), It.IsAny<int?>(), null, default(CancellationToken)))
                .ThrowsAsync(new ErrorException
                {
                    Response = new HttpResponseMessageWrapper(new HttpResponseMessage(HttpStatusCode.NotFound), null),
                    Body = new Error
                    {
                        Code = "not_found",
                        Status = 404
                    }
                });

            var ex = Assert.Throws<DataException>(() => _repository.Find("foo"));
        }

        [Fact]
        public void Find_TooManyRequests()
        {
            //Return 429 - Too many requests for search
            _scryfallClient.Setup(m => m.Cards.SearchWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<UniqueStrategy?>(), It.IsAny<SortOrder?>(), It.IsAny<SortDirection?>(), It.IsAny<bool?>(), It.IsAny<int?>(), null, default(CancellationToken)))
                .ThrowsAsync(new ErrorException()
                {
                    Response = new HttpResponseMessageWrapper(new HttpResponseMessage((HttpStatusCode)429), null)
                });

            var ex = Assert.Throws<DataException>(() => _repository.Find(CARD_NAME));
            Assert.True(ex.Fatal);
        }
    }
}
