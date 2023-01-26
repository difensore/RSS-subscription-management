using DAL.Models;

namespace RssSubscriptionManagement.Interfaces
{
    public interface IDataProvider
    {
        public Dictionary<Rssfeed, List<Item>> GetFeed();
        public Task<List<Item>> GetAllItemsbyDate(DateTime date,string User);
    }
}
