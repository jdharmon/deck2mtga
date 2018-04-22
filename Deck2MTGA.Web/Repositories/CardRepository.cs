using Deck2MTGA.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Scryfall.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deck2MTGA.Web.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromHours(24);
        private readonly string _legalSets;

        private IMemoryCache _cache;
        private IScryfallClient _scryfallClient;

        public CardRepository(IMemoryCache cache, IScryfallClient scryfallClient)
        {
            _cache = cache;
            _scryfallClient = scryfallClient;

            //Parse contents of LEGAL_SETS environment variable into search options format: (e:SET1 OR e:SET2)
            var legalSets = Environment.GetEnvironmentVariable("LEGAL_SETS");
            if (!string.IsNullOrEmpty(legalSets)) {
                var legalSetArray = legalSets.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                _legalSets = string.Join(" OR ", legalSetArray.Select(s => $"e:{s}").ToArray());
            }
        }

        public Card Find(string name)
        {
            return _cache.GetOrCreate(name.ToUpper(), (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeout;
                return Search(name);
            });
        }

        private Card Search(string name)
        {
            //Search for exact card name in legal sets
            var card = _scryfallClient.Cards.Search($"!\"{name}\" ({_legalSets})").Data.FirstOrDefault();

            //Add sleep to calls so we don't exceed the API's rate limit
            Thread.Sleep(50);

            return new Card()
            {
                Name = card.Name,
                Set = card.Set.ToUpper(),
                CollectorNumber = int.TryParse(card.CollectorNumber, out int n) ? n : 0
            };
        }
    }
}
