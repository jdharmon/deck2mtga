using Deck2MTGA.Web.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Rest;
using Moq;
using Scryfall.API;
using Scryfall.API.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Deck2MTGA.Tests
{
    public class CardRepositoryTests
    {
        const string CARD_NAME = "Opt";

        private readonly Mock<IScryfallClient> _scryfallClient;
        private readonly CardRepository _repository;

        public CardRepositoryTests()
        {
            _scryfallClient = new Mock<IScryfallClient>();
            SetupScryfallClientResponses();
            _repository = new CardRepository(new MemoryCache(new MemoryCacheOptions()), _scryfallClient.Object);
        }

        private void SetupScryfallClientResponses()
        {
            //Return mix of legal and illegal sets
            _scryfallClient.Setup(m => m.Sets.GetAllWithHttpMessagesAsync(null, default(CancellationToken)))
                .ReturnsAsync(new HttpOperationResponse<SetList>
                {
                    Response = new HttpResponseMessage(HttpStatusCode.OK),
                    Body = new SetList(new[]  
                    {
                        new Set
                        {   //Legal
                            Code = "akh",
                            SetType = SetTypes.Expansion,
                            ReleasedAt = new DateTime(2017, 04, 28)
                        },
                        new Set
                        {   //Legal
                            Code = "m19",
                            SetType = SetTypes.Core,
                            ReleasedAt = new DateTime(2018, 01, 01)
                        },
                        new Set
                        {   //Illegal - wrong type
                            Code = "ddu",
                            SetType = SetTypes.DuelDeck,
                            ReleasedAt = new DateTime(2018, 04, 06)
                        },
                        new Set
                        {   //Illegal - released too early
                            Code = "aer",
                            SetType = SetTypes.Expansion,
                            ReleasedAt = new DateTime(2017, 01, 20)
                        },
                        new Set
                        {   //Illegal - unlreleased released too early
                            Code = "aer",
                            SetType = SetTypes.Expansion,
                            ReleasedAt = new DateTime(2099, 01, 20)
                        }
                    })
                });

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

        [Fact]
        public void GetLegalSets()
        {
            var expected = new[] { "akh", "m19" };
            var actual = _repository.GetLegalSets();

            Assert.Equal(expected, actual);
        }
    }
}
