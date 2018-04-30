using Deck2MTGA.Web.Models;
using MongoDB.Driver;
using System.Linq;

namespace Deck2MTGA.Web.Repositories
{
    public interface IMtgDbContext
    {
        IQueryable<Card> Cards { get; }
        IQueryable<Set> Sets { get; }
    }
}