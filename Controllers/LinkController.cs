using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShortLinkAppV2._0.DTO;
using ShortLinkAppV2._0.Interfaces;

namespace ShortLinkAppV2._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkController : Controller
    {
        private ILinkService _linkService;
        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost("CreateShortLink")]
        public JsonResult CreateShortLink([FromBody]string fullLink)
        {
            var shortLink = _linkService.CreateShortLinkAsync(new URI { FullURI = fullLink, Created = DateTime.Now }).Result;
            return new JsonResult(shortLink);
        }

        [HttpPost("FindFullLink")]
        public JsonResult FindFullLink([FromBody]string token)
        {
            var fullLink = _linkService.ReadUriAsync(token).Result;
            return new JsonResult(fullLink);
        }

        [HttpGet("FindAllLinks")]
        public JsonResult FindAllLinks()
        {
            var fullLinks = _linkService.ReadUriAsync().Result;
            return new JsonResult(fullLinks);
        }
    }
}