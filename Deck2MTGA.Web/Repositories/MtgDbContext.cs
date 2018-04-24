using Deck2MTGA.Web.Models;
using MongoDB.Driver;

namespace Deck2MTGA.Web.Repositories
{
    public class MtgDbContext : IMtgDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MtgDbContext()
        {
            var client = new MongoClient("mongodb://localhost");
            if (client != null)
                _database = client.GetDatabase("mtg");
        }

        public IMongoCollection<Card> Cards
        {
            get
            {
                return _database.GetCollection<Card>("cards");
            }
        }

        public IMongoCollection<Set> Sets
        {
            get => _database.GetCollection<Set>("sets");
        }
    }
}
