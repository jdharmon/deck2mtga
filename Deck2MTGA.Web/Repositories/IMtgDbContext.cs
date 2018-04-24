using Deck2MTGA.Web.Models;
using MongoDB.Driver;

namespace Deck2MTGA.Web.Repositories
{
    public interface IMtgDbContext
    {
        IMongoCollection<Card> Cards { get; }
        IMongoCollection<Set> Sets { get; }
    }
}