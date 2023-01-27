namespace RssSubscriptionManagement.Interfaces
{
    public interface IFeedsConvertor
    {
        public Task<string> GetAllItems();
        public Task<string> GetFeedsUnread(DateTime date, string User);
        public Task<string> AddFeedtoDB(string url);
    }
}
