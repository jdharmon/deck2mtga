using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Deck2MTGA.Web.Models;
using Deck2MTGA.Web.Repositories;

namespace Deck2MTGA.Web
{
    public class Deck
    {
        private static Regex _cardRegex = new Regex(@"(?<count>\d+)x?\s+(?<name>[^\r\n]+)", RegexOptions.Compiled);
        private readonly ICardRepository _cardRepository;

        public List<Card> Cards { get; } = new List<Card>();
        public List<string> Errors { get; } = new List<string>();

        public Deck(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public void Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;
                
            ParseAsync(input).GetAwaiter().GetResult();
        }

        public async Task ParseAsync(string input)
        {
            using (var reader = new StringReader(input)) {
                string line;
                while ((line = await reader.ReadLineAsync()) != null) {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var match = _cardRegex.Match(line);
                    if (match.Success) {
                        try
                        {
                            var card = _cardRepository.Find(match.Groups["name"].Value);
                            card.Count = int.Parse(match.Groups["count"].Value);
                            Cards.Add(card);
                        }
                        catch (DataException ex)
                        {
                            if (!ex.Fatal)
                                Errors.Add($"{line} - {ex.Message}");
                            else
                                throw;
                        }
                    }
                    else
                        Errors.Add($"{line} - Invalid format");
                }
            }
        }

        public string ToDeckString() {
            var builder = new StringBuilder();
            foreach (var card in Cards)
                builder.AppendLine(card.ToString());
            return builder.ToString();
        }

        public string ToArenaString() {
            var builder = new StringBuilder();
            foreach (var card in Cards)
                builder.AppendLine(card.ToArenaString());
            return builder.ToString();
        }
    }
}