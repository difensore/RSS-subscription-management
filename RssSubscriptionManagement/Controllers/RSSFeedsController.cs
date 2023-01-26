using Microsoft.AspNetCore.Mvc;
using RssSubscriptionManagement.Interfaces;

namespace RssSubscriptionManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RSSFeedsController : ControllerBase
    {
        private readonly IFeedsConvertor _convertor;
        public RSSFeedsController(IFeedsConvertor converotr)
        {
            _convertor= converotr;
        }
        [HttpGet, Route("rss")]
        public async Task<IActionResult> Rss()
        {
            string host = Request.Scheme + "://" + Request.Host;
            string contentType = "application/xml";

            var content = await _convertor.GetFeeds();
            return Content(content, contentType);
        }
    }
}
