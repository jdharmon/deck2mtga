using Deck2MTGA.Web.Models;

namespace Deck2MTGA.Web.Repositories
{
    public interface ICardRepository
    {
        Card Find(string name);
    }
}