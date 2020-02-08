using ShortLinkAppV2._0.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkAppV2._0.Interfaces
{
    public interface ILinkService
    {
        /// <returns>short link</returns>
        string CreateShortLink(URI uri);

        URI ReadUri(string token);

        Task<URI> ReadUriAsync(string token);

        Task<string> CreateShortLinkAsync(URI uri);

        /// <returns>all uries</returns>
        List<URI> ReadUri();

        /// <returns>all uries</returns>
        Task<List<URI>> ReadUriAsync();
    }
}
