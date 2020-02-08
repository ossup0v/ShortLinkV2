using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace ShortLinkAppV2._0.DTO
{
    [Serializable]
    [BsonIgnoreExtraElements]
    [DataContract]
    public class Entry
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [DataMember]
        public string Id
        {
            get { return _id.ToString(); }
            set { _id = ObjectId.Parse(value); }
        }

        public URI Uri { get; set; }

    }
    public enum EntryField
    {
        EntryId = 0,
        Uri = 1,
        UriFullURI = 2,
        UriShortURI = 3,
        UriToken = 4,
        UriClicked = 5,
        UriCreated = 6,
        UriCreater = 7
    }
}
