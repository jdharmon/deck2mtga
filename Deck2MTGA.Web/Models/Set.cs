﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deck2MTGA.Web.Models
{
    public class Set
    {
        [BsonId]
        // standard BSonId generated by MongoDb
        public ObjectId InternalId { get; set; }

        [BsonElement("alternativeNames")]
        public string[] AlternativeNames { get; set; }

        [BsonElement("block")]
        public string Block { get; set; }

        [BsonElement("booster")]
        public object Booster { get; set; }

        [BsonElement("border")]
        public string Border { get; set; }

        [BsonElement("cards")]
        public object[] Cards { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("essentialMagicCode")]
        public string EssentialMagicCode { get; set; }

        [BsonElement("gathererCode")]
        public string GathererCode { get; set; }

        [BsonElement("isMCISet")]
        public bool IsMciSet { get; set; }

        [BsonElement("language")]
        public string Language { get; set; }

        [BsonElement("magicCardsInfoCode")]
        public string MagicCardsInfoCode { get; set; }

        [BsonElement("magicRaritiesCodes")]
        public string[] MagicRaritiesCodes { get; set; }

        [BsonElement("mkm_id")]
        public int MkmId { get; set; }

        [BsonElement("mkm_name")]
        public string MkmName { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("oldCode")]
        public string OldCode { get; set; }

        [BsonElement("onlineOnly")]
        public bool OnlineOnly { get; set; }

        [BsonElement("releaseDate")]
        public string ReleaseDate { get; set; }

        [BsonElement("translations")]
        public object Translations { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("useMagicRaritiesNumber")]
        public bool UseMagicRaritiesNumber { get; set; }
    }
}
