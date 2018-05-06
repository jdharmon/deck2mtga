using Microsoft.Extensions.Caching.Memory;
using Scryfall.API;
using Scryfall.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Deck2MTGA.Web.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromHours(24);

        private IMemoryCache _cache;
        private IScryfallClient _scryfallClient;

        public CardRepository(IMemoryCache cache, IScryfallClient scryfallClient)
        {
            _cache = cache;
            _scryfallClient = scryfallClient;
        }

        /// <summary>
        /// Find card by name
        /// </summary>
        /// <remarks>Uses caching to limit API requests</remarks>
        /// <param name="name"></param>
        /// <returns>Card</returns>
        public Card Find(string name)
        {
            return _cache.GetOrCreate(name.ToUpper(), (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeout;
                return Search(name);
            });
        }

        /// <summary>
        /// Search Scryfall for card by name
        /// </summary>
        /// <param name="name">Name of card to find</param>
        /// <returns>Card</returns>
        private Card Search(string name)
        {
            try
            {
                var legalSets = GetLegalSetSearchString();
                //Search for exact card name in legal sets
                var card = _scryfallClient.Cards.Search($"!\"{name}\" ({legalSets})").Data.First();

                //Add sleep to calls so we don't exceed the API's rate limit
                Thread.Sleep(50);

                return new Card()
                {
                    Name = card.Name,
                    Set = card.Set.ToUpper(),
                    CollectorNumber = int.TryParse(card.CollectorNumber, out int n) ? n : 0
                };
            }
            catch (Scryfall.API.Models.ErrorException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    throw new DataException("Card not found");

                throw new DataException("Unexpected error searching for card", ex, true);
            }
        }

        /// <summary>
        /// Get list of legal sets as search string
        /// </summary>
        /// <returns>Set search string</returns>
        private string GetLegalSetSearchString()
        {
            return _cache.GetOrCreate("LegalSets", (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeout;

                var legalSets = GetLegalSets();
                return string.Join(" OR ", legalSets.Select(s => $"e:{s}").ToArray());
            });
        }


        /// <summary>
        /// Get list of legal sets
        /// </summary>
        /// <returns>Legal sets</returns>
        public IEnumerable<string> GetLegalSets()
        {
            return _scryfallClient.Sets.GetAll().Data
                    .Where(s => s.ReleasedAt >= new DateTime(2017, 04, 28) && s.ReleasedAt <= DateTime.Today)
                    .Where(s => s.SetType == SetTypes.Core || s.SetType == SetTypes.Expansion)
                    .Select(s => s.Code);
        }
    }
}
