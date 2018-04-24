using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deck2MTGA.Web.Models
{
    public class Card
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        [BsonElement("artist")]
        public string Artist { get; set; }

        [BsonElement("border")]
        public string Border { get; set; }

        [BsonElement("cmc")]
        public decimal Cmc { get; set; }

        [BsonElement("colorIdentity")]
        public string[] ColorIdentity { get; set; }

        [BsonElement("colors")]
        public string[] Colors { get; set; }

        [BsonElement("flavor")]
        public string Flavor { get; set; }

        [BsonElement("foreignNames")]
        public object[] ForeignNames { get; set; }

        [BsonElement("hand")]
        public int Hand { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("imageName")]
        public string ImageName { get; set; }

        [BsonElement("layout")]
        public string Layout { get; set; }

        [BsonElement("legalities")]
        public object[] Legalities { get; set; }

        [BsonElement("life")]
        public int Life { get; set; }

        [BsonElement("loyalty")]
        public int Loyalty { get; set; }

        [BsonElement("manaCost")]
        public string ManaCost { get; set; }

        [BsonElement("mciNumber")]
        public string MciNumber { get; set; }

        [BsonElement("multiverseid")]
        public int MultiverseId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("names")]
        public string[] Names { get; set; }

        [BsonElement("number")]
        public string Number { get; set; }

        [BsonElement("originalText")]
        public string OriginalText { get; set; }

        [BsonElement("originalType")]
        public string OriginalType { get; set; }

        [BsonElement("power")]
        public string Power { get; set; }

        [BsonElement("printings")]
        public string[] Printings { get; set; }

        [BsonElement("rarity")]
        public string Rarity { get; set; }

        [BsonElement("releaseDate")]
        public string ReleaseDate { get; set; }

        [BsonElement("reserved")]
        public bool Reserved { get; set; }

        [BsonElement("rulings")]
        public object[] Rulings { get; set; }

        [BsonElement("set")]
        public string Set { get; set; }

        [BsonElement("source")]
        public string Source { get; set; }

        [BsonElement("starter")]
        public bool Starter { get; set; }

        [BsonElement("subtypes")]
        public string[] Subtypes { get; set; }

        [BsonElement("supertypes")]
        public string[] Supertypes { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("timeshifted")]
        public bool Timeshifted { get; set; }

        [BsonElement("toughness")]
        public string Toughness { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("types")]
        public string[] Types { get; set; }

        [BsonElement("variations")]
        public int[] Variations { get; set; }

        [BsonElement("watermark")]
        public string Watermark { get; set; }

        public int Count { get; set; }

        override public string ToString()
        {
            return $"{Count} {Name}";
        }

        public string ToArenaString()
        {
            return $"{Count} {Name} ({Set}) {Number}";
        }

    }
}
