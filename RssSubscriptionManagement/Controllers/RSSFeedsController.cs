using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSubscriptionManagement.Interfaces;
using System.Security.Claims;

namespace RssSubscriptionManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RSSFeedsController : ControllerBase
    {
        private readonly IFeedsConvertor _convertor;
        private readonly IDataProvider _dp;
        public RSSFeedsController(IFeedsConvertor converotr,IDataProvider dp)
        {
            _convertor = converotr;
            _dp = dp;
        }
        /*  [HttpGet, Route("rss")]
          public async Task<IActionResult> Rss()
          {
              string host = Request.Scheme + "://" + Request.Host;
              string contentType = "application/xml";

              var content = await _convertor.GetFeeds();
              return Content(content, contentType);
          }*/

        [HttpGet, Route("rssbydate")]
        public async Task<IActionResult> RssbyDate(string date)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user == null)
            {
                return Content("You must sign in");
            }
            string host = Request.Scheme + "://" + Request.Host;
            string contentType = "application/xml";

            var content = await _convertor.GetFeedsUnread(DateTime.Parse(date), user);
            return Content(content, contentType);
        }
        [Authorize]
        [HttpPost, Route("rssmark")]
        public async Task<IActionResult> SetAsRead(string url)
        {
            var result =await _dp.SetitemAsRead(url, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Content(result);
        }
        [HttpPost, Route("newrss")]
        public async Task<IActionResult> Getrss(string url)
        {
            var result = await _convertor.AddFeedtoDB(url);
            return Content(result);
        }

        [HttpGet, Route("allfeeds")]
        public IActionResult GetFeeds()
        {            
            return Content(_dp.GetFeeds());
        }
    }
}
