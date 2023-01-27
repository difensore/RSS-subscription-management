using DAL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using RssSubscriptionManagement.Interfaces;
using System;
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
                string Summary;
                string link;
                string author;
                try
                {
                    author = post.Authors.First().Name;
                }
                catch
                {
                    author = "Unknown";
                }
                try
                {
                    link = post.Links.First().Uri.ToString();
                }
                catch
                {
                    link = "No link";
                }
                try
                {
                    Summary = post.Summary.Text;
                }
                catch
                {
                    Summary = null;
                }
                Item item = new Item() { Id = Guid.NewGuid().ToString(), Title = post.Title.Text, Description =Summary, Author = author, Link = link, Date = post.PublishDate.Date };
                items.Add(item);
            }
            string linkfeed;
            try
            {
                linkfeed = feed.Links.First().Uri.ToString();
            }
            catch
            {
                linkfeed = "No link";
            }
            Rssfeed rssfeed = new Rssfeed() { Id = Guid.NewGuid().ToString(), Title = feed.Title.Text, Description = feed.Description.Text, Link =linkfeed};
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
            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").GetUtcOffset(post.Date);
            var item = new AtomEntry
            {
                Title = post.Title,
                Description = post.Description,
                Id = post.Link,
                Published = new DateTimeOffset(post.Date, offset),
                LastUpdated = post.Date,
                ContentType = "html"
            };
            item.AddContributor(
                new SyndicationPerson(post.Author, post.Author));

            item.AddLink(
                new SyndicationLink(new Uri(item.Id)));

            return item;
        }
    }
}
