namespace Deck2MTGA.Web.Repositories
{
    public interface ICardRepository
    {
        Card Find(string name);
    }
}