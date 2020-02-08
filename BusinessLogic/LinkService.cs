using ShortLinkAppV2._0.DTO;
using ShortLinkAppV2._0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkAppV2._0.BusinessLogic
{
    public class LinkService : ILinkService
    {
        private const string BASE_URI = "https://localhost:44365/q?token=";
        private ILinkStorage _storage;
        private static string _tokenDefault = "";
        private static string _idDefault = "";
        private string _token = _tokenDefault;
        private string _id = _idDefault;

        public LinkService(ILinkStorage storage)
        {
            _storage = storage;
        }

        #region API

        //ToDo: use usercontext
        public string CreateShortLink(URI Uri)
        {
            //var ctx = UserContext.GetContext();
            GenerateShortLink(Uri);
            //if (ctx != default(UserContext))
            //{
            //    return CreateShortLink(Uri, ctx);
            //}
            var entry = new Entry { Uri = Uri, Id = Uri.Id };
            _storage.Create(entry);
            return Uri.ShortURI;
        }

        public URI ReadUri(string token)
        {
            //var ctx = UserContext.GetContext();
            var entry = _storage.Read(EntryField.UriToken, token)?.FirstOrDefault();
            //if (entry == null)
            //{
            //    return null;
            //}
            _storage.Update(EntryField.UriToken, token, EntryField.UriClicked, entry.Uri.Clicked + 1);
            var uri = entry?.Uri;
            return uri;
        }

        public List<URI> ReadUri()
        {
            var entries = _storage.Read();
            var result = new List<URI>();
            foreach (var entry in entries)
            {
                result.Add(entry.Uri);
            }
            return result;
        }

        #endregion

        #region Async API

        public async Task<string> CreateShortLinkAsync(URI uri)
        {
            GenerateShortLink(uri);
            var entry = new Entry { Uri = uri, Id = uri.Id };
            await _storage.CreateAsync(entry);
            return uri.ShortURI;
        }

        public async Task<URI> ReadUriAsync(string token)
        {
            var entry = (await _storage.ReadAsync(EntryField.UriToken, token))?.FirstOrDefault();
            if (entry == null)
            {
                return null;
            }
            await _storage.UpdateAsync(EntryField.UriToken, token, EntryField.UriClicked, entry.Uri.Clicked + 1);
            var uri = entry?.Uri;
            return uri;
        }

        public async Task<List<URI>> ReadUriAsync()
        {
            var entries = await _storage.ReadAsync();
            var result = new List<URI>();
            foreach (var entry in entries)
            {
                result.Add(entry.Uri);
            }
            return result;
        }

        #endregion

        private void GenerateShortLink(URI uri)
        {
            var tokensAndIds = FindAllTokensAndIds();
            while (tokensAndIds.Exists(t => t.Item1 == _token) || (_token == _tokenDefault))
            {
                GenerateToken();
            }
            while (tokensAndIds.Exists(t => t.Item2 == _id) || (_id == _idDefault))
            {
                GenerateId();
            }
            uri.Token = _token;
            uri.Id = _id;
            uri.ShortURI = BASE_URI + _token;
        }

        private void GenerateId()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(10, 8)
              .OrderBy(o => new Random().Next(9, 25))
              .ToList()
              .ForEach(i => urlsafe += i.ToString());
            var temp = Convert.ToInt64(urlsafe, 16).ToString();
            for (int i = temp.Length; i < 25; i++)
            {
                temp += new Random().Next(0, 9);
            }
            _id = temp.ToString().Substring(0, 24);
        }

        private void GenerateToken()
        {
            _token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            if (_token.Contains('+') || _token.Contains('?') || _token.Contains('=') || _token.Contains('/'))
            {
                string temp = "";
                for (int i = 0; i < _token.Length; i++)
                {
                    if (_token[i] == '+' || _token[i] == '?' || _token[i] == '=' || _token[i] == '/')
                    {
                        temp += i.ToString();
                        continue;
                    }
                    temp += _token[i];
                }
                _token = temp;
            }
        }

        private List<(string, string)> FindAllTokensAndIds()
        {
            var entries = _storage.Read();
            var tokensAndIds = new List<(string, string)>();
            foreach (var entry in entries)
            {
                tokensAndIds.Add((entry.Uri.Token, entry.Id));
            }
            return tokensAndIds;
        }
    }
}
