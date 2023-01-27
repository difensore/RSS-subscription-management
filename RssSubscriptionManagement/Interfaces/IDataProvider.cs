using DAL.Models;

namespace RssSubscriptionManagement.Interfaces
{
    public interface IDataProvider
    {
        public Task<List<Item>> GetItems();
        public Task<List<Item>> GetAllItemsbyDate(DateTime date,string User);
        public Task<string> SetitemAsRead(string url, string user);
        public Task<string> AddFeed(Dictionary<Rssfeed, List<Item>> feeds);
        public string GetFeeds();
    }
}
