using DAL.Models;
using Microsoft.EntityFrameworkCore;
using RssSubscriptionManagement.Interfaces;
using System.Collections.Generic;

namespace RssSubscriptionManagement.Services
{
    public class DataProvider : IDataProvider
    {
        private readonly RssSubscriptionManagementContext db;
        public DataProvider(RssSubscriptionManagementContext context)
        {
            db = context;
        }
        public async Task<List<Item>> GetItems()
        {
           var items= await db.Items.ToListAsync();
            return items;
        }
        public async Task<List<Item>> GetAllItemsbyDate(DateTime date, string User)
        {
            var allItems = db.Items;
            var Watched = db.WatchedRsss.Where(p => p.UserId == User);
            var WatchedItems = Watched.Join(allItems, p => p.ItemId, c => c.Id, (p, c) => c);
            var unwatched = allItems.Except(WatchedItems);
            var UnwatchedbyDate = await unwatched.Where(p => p.Date == date).ToListAsync();
            return UnwatchedbyDate;
        }
        public async Task<string> SetitemAsRead(string url,string user)
        {
            try
            {
                var item = await db.Items.FirstAsync(p => p.Link == url);
                WatchedRss watched = new WatchedRss() { Id = Guid.NewGuid().ToString(), ItemId = item.Id, UserId = user };
                try
                {
                    var test = await db.WatchedRsss.Where(p => p.UserId == user).FirstAsync(p => p.ItemId == item.Id);
                    return "It's already marked as readed";
                }  
                catch
                {

                }
                db.WatchedRsss.Add(watched);
                db.SaveChanges();
                return "successfully marked as readed";
            }
            catch
            {
                return "Oops,some error";
            }
            
        }
        public async Task<string> AddFeed(Dictionary<Rssfeed,List<Item>> feeds)
        {
            try
            {
                List<FeedItem> items = new List<FeedItem>();
                var feed = feeds.First();
                db.Rssfeeds.Add(feed.Key);
                await db.AddRangeAsync(feed.Value);
                foreach (var item in feed.Value)
                {
                    FeedItem fi = new FeedItem() { Id = Guid.NewGuid().ToString(), FeedId = feed.Key.Id, ItemId = item.Id };
                    items.Add(fi);
                }
                await db.FeedItems.AddRangeAsync(items);
                db.SaveChanges();
                return "Done";
            }
            catch
            {
                return null;
            }           
        }
        public string GetFeeds()
        {
            string result=null;
            var feeds = db.Rssfeeds;
            foreach (var item in feeds)
            {
                result += item.Title + "\n";
            }
            return result;
        }
    }
}
