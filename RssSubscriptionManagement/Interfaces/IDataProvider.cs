using DAL.Models;

namespace RssSubscriptionManagement.Interfaces
{
    public interface IDataProvider
    {
        public Dictionary<Rssfeed, List<Item>> GetFeed();
        public async Task<List<Item>> GetAllItemsbyDate(DateOnly date)
    }
}
