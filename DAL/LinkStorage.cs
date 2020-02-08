using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShortLinkAppV2._0.DTO;
using ShortLinkAppV2._0.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkAppV2._0.DAL
{
    public class LinkStorage : ILinkStorage
    {
        private const string _URLDatabaseConnection = "mongodb://localhost:27017";
        private const string _databaseName = "LinkStorage";
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Entry> _checkCollection;

        private IMongoCollection<Entry> _collection
        {
            get
            {
                if (_checkCollection != null)
                    return _checkCollection;
                else
                    return _database.GetCollection<Entry>(_databaseName);
            }

            set
            { _checkCollection = value; }
        }

        public LinkStorage()
        {
            Configure();
        }

        #region API

        public void Create(Entry entry)
        {
            _collection.InsertOne(entry);
        }

        public IList<Entry> Read()
        {
            var entries = _collection.Find(new BsonDocument()).ToList();
            return entries;
        }

        public IList<Entry> Read(EntryField field, object value)
        {
            if (value == null)
                return null;
            var filter = CreateEqFilter(field, value);
            var entries = _collection.Find(filter).ToList();
            return entries;
        }

        public void Update(EntryField filterField, object filterValue, EntryField updateField, object updateValue)
        {
            var filter = CreateEqFilter(filterField, filterValue);
            var update = CreateSetUpdate(updateField, updateValue);
            _collection.UpdateMany(filter, update);
        }

        public bool Remove()
        {
            throw new NotImplementedException();
        }

        public bool Remove(EntryField Field, object value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Async API

        public async Task CreateAsync(Entry entry)
        {
            await _collection.InsertOneAsync(entry);
        }

        public async Task<IList<Entry>> ReadAsync()
        {
            var entries = (await _collection.FindAsync(new BsonDocument())).ToList();
            return entries;
        }

        public async Task<IList<Entry>> ReadAsync(EntryField field, object value)
        {
            if (value == null)
                return null;
            var filter = CreateEqFilter(field, value);
            var entries = (await _collection.FindAsync(filter)).ToList();
            return entries;
        }

        public async Task UpdateAsync(EntryField filterField, object filterValue, EntryField updateField, object updateValue)
        {
            var filter = CreateEqFilter(filterField, filterValue);
            var update = CreateSetUpdate(updateField, updateValue);
            await _collection.UpdateManyAsync(filter, update);
        }

        #endregion

        private UpdateDefinition<Entry> CreateSetUpdate(EntryField field, object value)
        {
            switch (field)
            {
                case EntryField.EntryId:
                    return Builders<Entry>.Update.Set(x => x.Id, value.ToString());
                case EntryField.Uri:
                    return Builders<Entry>.Update.Set(x => x.Uri, (URI)value);
                case EntryField.UriFullURI:
                    return Builders<Entry>.Update.Set(x => x.Uri.FullURI, value.ToString());
                case EntryField.UriShortURI:
                    return Builders<Entry>.Update.Set(x => x.Uri.ShortURI, value.ToString());
                case EntryField.UriToken:
                    return Builders<Entry>.Update.Set(x => x.Uri.Token, value.ToString());
                case EntryField.UriClicked:
                    return Builders<Entry>.Update.Set(x => x.Uri.Clicked, (int)value);
                case EntryField.UriCreated:
                    return Builders<Entry>.Update.Set(x => x.Uri.Created, (DateTime)value);
                case EntryField.UriCreater:
                    return Builders<Entry>.Update.Set(x => x.Uri.Creater, value.ToString());
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private FilterDefinition<Entry> CreateEqFilter(EntryField field, object value)
        {
            switch (field)
            {
                case EntryField.EntryId:
                    return Builders<Entry>.Filter.Eq(x => x.Id, value.ToString());
                case EntryField.Uri:
                    return Builders<Entry>.Filter.Eq(x => x.Uri, (URI)value);
                case EntryField.UriFullURI:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.FullURI, value.ToString());
                case EntryField.UriShortURI:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.ShortURI, value.ToString());
                case EntryField.UriToken:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.Token, value.ToString());
                case EntryField.UriClicked:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.Clicked, (int)value);
                case EntryField.UriCreated:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.Created, (DateTime)value);
                case EntryField.UriCreater:
                    return Builders<Entry>.Filter.Eq(x => x.Uri.Creater, value.ToString());
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private void Configure()
        {
            try
            {
                _client = new MongoClient(_URLDatabaseConnection);
                _database = _client.GetDatabase(_databaseName);
                BsonClassMap.RegisterClassMap<Entry>(cm =>
                {
                    cm.AutoMap();
                });
            }
            catch
            {

            };
        }
    }
}
