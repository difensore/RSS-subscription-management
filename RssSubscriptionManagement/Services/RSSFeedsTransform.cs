using DAL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using RssSubscriptionManagement.Interfaces;
using System.Xml;

namespace RssSubscriptionManagement.Services
{
    public class RSSFeedsTransform:IFeedsConvertor
    {
        private readonly IDataProvider _dp;
        public RSSFeedsTransform(IDataProvider dp)
        {
            _dp=dp;
        }
        public async Task<string> GetFeeds()
        {
            string result=null;
            var dict = _dp.GetFeed();
            
            StringWriter sw = new StringWriter();
            
                using (XmlWriter xmlWriter = XmlWriter.Create(
                        sw, new XmlWriterSettings()
                        {
                            Async = true,
                            Indent = true
                        }))
                foreach (var element in dict)
                {
                    {
                    var rss = new RssFeedWriter(xmlWriter);
                    await rss.WriteTitle(element.Key.Title);
                    await rss.WriteDescription(element.Key.Description);                    
                    await rss.WriteValue("link", element.Key.Link);

                    if (element.Value != null && element.Value.Count() > 0)
                    {                        
                        var feedItems = new List<AtomEntry>();
                        foreach (var post in element.Value)
                        {
                            var item = ToRssItem(post);
                            feedItems.Add(item);
                        }

                        foreach (var feedItem in feedItems)
                        {
                            await rss.Write(feedItem);
                        }
                        }
                    }
                }
                    return sw.ToString();              
        }
        private AtomEntry ToRssItem(Item post)
        {            
            var item = new AtomEntry
            {
                Title = post.Title,
                Description = post.Description,
                Id = post.Link,
                Published =post.Date,
                LastUpdated = post.Date,
                ContentType = "html",
            };

           /* if (!string.IsNullOrEmpty(post.Category))
            {
                item.AddCategory(
                    new SyndicationCategory(post.Category));
            }*/

            item.AddContributor(
                new SyndicationPerson(post.Author, post.Author));

            item.AddLink(
                new SyndicationLink(new Uri(item.Id)));

            return item;
        }
    }
}
