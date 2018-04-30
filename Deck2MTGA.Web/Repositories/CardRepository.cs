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
        private readonly string[] _legalSets = new string[0];

        private IMtgDbContext _dbContext;

        public CardRepository(IMtgDbContext dbContext)
        {
            _dbContext = dbContext;

            //Parse contents of LEGAL_SETS environment variable into search options format: (e:SET1 OR e:SET2)
            var legalSets = Environment.GetEnvironmentVariable("LEGAL_SETS");
            if (!string.IsNullOrEmpty(legalSets))
            {
                _legalSets = legalSets.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public Card Find(string name)
        {
            return _dbContext.Cards
                .Where(c => c.Name == name)
                .Where(c => _legalSets.Length == 0 || _legalSets.Contains(c.Set))
                .OrderByDescending(c => c.MultiverseId)
                .FirstOrDefault();
        }
    }
}
