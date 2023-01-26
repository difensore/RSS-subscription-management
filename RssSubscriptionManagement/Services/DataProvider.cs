using DAL.Models;
using Microsoft.EntityFrameworkCore;
using RssSubscriptionManagement.Interfaces;

namespace RssSubscriptionManagement.Services
{
    public class DataProvider:IDataProvider
    {
        private readonly RssSubscriptionManagementContext db;
        public DataProvider(RssSubscriptionManagementContext context)
        {
            db= context;
        }
        public Dictionary<Rssfeed, List<Item>> GetFeed()
        {
            Dictionary<Rssfeed, List<Item>> some = new Dictionary<Rssfeed, List<Item>>();
            var Feeds = db.Rssfeeds;
            foreach(var Feed in Feeds)
            {
                var itemfeeds= db.FeedItems.Where(p=>p.FeedId==Feed.Id);
                var items=db.Items.Join(itemfeeds,p=>p.Id,c=>c.ItemId,(p,c)=>p).ToList();                
                some.Add(Feed, items);
            }
            return some;
        }      
        public async Task<List<Item>> GetAllItemsbyDate(DateTime date)
        {
            var allItems =db.Items;
            var items = await db.WatchedRsss.Join(allItems, p => p.ItemId, c => c.Id, (p, c) => c).ToListAsync();
            var itemsbyDate = items.Where(p => p.Date == date);
            return items;
        }
    }
}
