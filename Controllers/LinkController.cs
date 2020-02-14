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
        public async Task<JsonResult> CreateShortLink([FromBody]string fullLink)
        {
            var shortLink = await _linkService.CreateShortLinkAsync(new URI { FullURI = fullLink, Created = DateTime.Now });
            return new JsonResult(shortLink);
        }

        [HttpPost("FindFullLink")]
        public async Task<JsonResult> FindFullLink([FromBody]string token)
        {
            var fullLink = await _linkService.ReadUriAsync(token);
            return new JsonResult(fullLink);
        }

        [HttpGet("FindAllLinks")]
        public async Task<JsonResult> FindAllLinksA()
        {
            var fullLinks = await _linkService.ReadUriAsync();
            return new JsonResult(fullLinks);
        }


        [HttpGet("~/q")]
        public async Task<IActionResult> Redirect(string token)
        {
            var uri = await _linkService.ReadUriAsync(token);
            return new RedirectResult(uri.FullURI);
        }
    }
}