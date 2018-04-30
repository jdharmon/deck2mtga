using Deck2MTGA.Web.Models;
using MongoDB.Driver;
using System.Linq;

namespace Deck2MTGA.Web.Repositories
{
    public class MtgDbContext : IMtgDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MtgDbContext()
        {
            var client = new MongoClient("mongodb://db");
            if (client != null)
                _database = client.GetDatabase("mtg");
        }

        public IQueryable<Card> Cards
        {
            get
            {
                return _database.GetCollection<Card>("cards").AsQueryable();
            }
        }

        public IQueryable<Set> Sets
        {
            get => _database.GetCollection<Set>("sets").AsQueryable();
        }
    }
}
