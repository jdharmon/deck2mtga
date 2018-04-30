using Deck2MTGA.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;

namespace Deck2MTGA.Web.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromHours(24);
        private readonly string _legalSets;

        private IMtgDbContext _dbContext;

        public CardRepository(IMtgDbContext dbContext)
        {
            _dbContext = dbContext;

            //Parse contents of LEGAL_SETS environment variable into search options format: (e:SET1 OR e:SET2)
            var legalSets = Environment.GetEnvironmentVariable("LEGAL_SETS");
            if (!string.IsNullOrEmpty(legalSets))
            {
                var legalSetArray = legalSets.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                _legalSets = string.Join(" OR ", legalSetArray.Select(s => $"e:{s}").ToArray());
            }
        }

        public Card Find(string name)
        {
            return _dbContext.Cards
                .Where(c => c.Name == name)
                .OrderByDescending(c => c.MultiverseId)
                .FirstOrDefault();
        }
    }
}
