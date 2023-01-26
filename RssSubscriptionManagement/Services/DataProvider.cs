using DAL.Models;
using Microsoft.EntityFrameworkCore;
using RssSubscriptionManagement.Interfaces;

namespace RssSubscriptionManagement.Services
{
    public class DataProvider : IDataProvider
    {
        private readonly RssSubscriptionManagementContext db;
        public DataProvider(RssSubscriptionManagementContext context)
        {
            db = context;
        }
        public Dictionary<Rssfeed, List<Item>> GetFeed()
        {
            Dictionary<Rssfeed, List<Item>> some = new Dictionary<Rssfeed, List<Item>>();
            var Feeds = db.Rssfeeds;
            foreach (var Feed in Feeds)
            {
                var itemfeeds = db.FeedItems.Where(p => p.FeedId == Feed.Id);
                var items = db.Items.Join(itemfeeds, p => p.Id, c => c.ItemId, (p, c) => p).ToList();
                some.Add(Feed, items);
            }
            return some;
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
    }
}
