using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Deck2MTGA_Web
{
    public class Deck
    {
        private static Regex _cardRegex = new Regex(@"(?<count>\d+)x?\s+(?<name>[^\r\n]+)", RegexOptions.Compiled);
        public List<Card> Cards { get; } = new List<Card>();
        public List<string> Errors { get; } = new List<string>();

        public static Deck Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
                
            return ParseAsync(input).GetAwaiter().GetResult();
        }

        public static async Task<Deck> ParseAsync(string input)
        {
            var deck = new Deck();
            using (var reader = new StringReader(input)) {
                string line;
                while ((line = await reader.ReadLineAsync()) != null) {
                    var match = _cardRegex.Match(line);
                    if (match.Success) {
                        var card = await FindCard(match.Groups["name"].Value);
                        if (card == null) {
                            deck.Errors.Add(line);
                            continue;
                        }
                        card.Count = int.Parse(match.Groups["count"].Value);
                        deck.Cards.Add(card);
                    }
                    else
                        deck.Errors.Add(line);
                    await Task.Delay(100);
                }
            }
            return deck;
        }
        public static async Task<Card> FindCard(string name)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.scryfall.com/");
            var response = await httpClient.GetAsync($"/cards/named?exact={name}");
            if (!response.IsSuccessStatusCode)
                return null;
            dynamic card = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return new Card() {
                Name = card.name,
                Set = card.set.ToString().ToUpper(),
                CollectorNumber = card.collector_number
            };
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