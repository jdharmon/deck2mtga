namespace Deck2MTGA.Web
{    
    public class Card {
        public string Name { get; set; }
        public string Set { get; set; }
        public int Count { get; set; }
        public int CollectorNumber { get; set; }

        override public string ToString() {
            return $"{Count} {Name}";
        }

        public string ToArenaString() {
            return $"{Count} {Name} ({Set}) {CollectorNumber}";
        }
    }
}