using DAL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using RssSubscriptionManagement.Interfaces;
using System.ServiceModel.Syndication;
using System.Xml;
using SyndicationLink = Microsoft.SyndicationFeed.SyndicationLink;
using SyndicationPerson = Microsoft.SyndicationFeed.SyndicationPerson;

namespace RssSubscriptionManagement.Services
{
    public class RSSFeedsTransform : IFeedsConvertor
    {
        private readonly IDataProvider _dp;
        public RSSFeedsTransform(IDataProvider dp)
        {
            _dp = dp;
        }
        public async Task<string> GetAllItems()
        {
            var items =await _dp.GetItems();
            StringWriter sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(
                    sw, new XmlWriterSettings()
                    {
                        Async = true,
                        Indent = true
                    }))

            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle("All news");
                await rss.WriteDescription($"All news that we have");
                await rss.WriteValue("link", "https://github.com/difensore");

                if (items != null && items.Count() > 0)
                {
                    var feedItems = new List<AtomEntry>();
                    foreach (var post in items)
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
            return sw.ToString();
        }
        public async Task<string> GetFeedsUnread(DateTime date, string User)
        {
            var items = _dp.GetAllItemsbyDate(date, User).Result;
            StringWriter sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(
                    sw, new XmlWriterSettings()
                    {
                        Async = true,
                        Indent = true
                    }))

            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle("Unread");
                await rss.WriteDescription($"Unread news by {date}");
                await rss.WriteValue("link", "https://github.com/difensore");

                if (items != null && items.Count() > 0)
                {
                    var feedItems = new List<AtomEntry>();
                    foreach (var post in items)
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
            return sw.ToString();
        }
        public async Task<string> AddFeedtoDB(string url)
        {
            Dictionary<Rssfeed, List<Item>> result = new Dictionary<Rssfeed, List<Item>>();
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            var posts = feed.Items;
            List<Item> items = new List<Item>();
            foreach (var post in posts)
            {
                string author;
                try
                {
                    author = post.Authors.First().Name;
                }
                catch
                {
                    author = "Unknown";
                }
                Item item = new Item() { Id = Guid.NewGuid().ToString(), Title = post.Title.Text, Description = post.Summary.Text, Author = author, Link = post.Links.First().Uri.ToString(), Date = post.PublishDate.Date };
                items.Add(item);
            }
            Rssfeed rssfeed = new Rssfeed() { Id = Guid.NewGuid().ToString(), Title = feed.Title.Text, Description = feed.Description.Text, Link = feed.Links.First().Uri.ToString() };
            result.Add(rssfeed, items);
            var answer = await _dp.AddFeed(result);
            if (answer != null)
            {
                return answer;
            }
            else
            {
                return "Something go wrong";
            }
        }

        private AtomEntry ToRssItem(Item post)
        {
            var item = new AtomEntry
            {
                Title = post.Title,
                Description = post.Description,
                Id = post.Link,
                Published = post.Date,
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
