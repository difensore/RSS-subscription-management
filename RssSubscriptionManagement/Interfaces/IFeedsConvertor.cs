namespace RssSubscriptionManagement.Interfaces
{
    public interface IFeedsConvertor
    {
        public Task<string> GetFeeds();
        public Task<string> GetFeedsUnread(DateTime date, string User);
    }
}
