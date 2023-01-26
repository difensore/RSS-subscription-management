namespace RssSubscriptionManagement.Interfaces
{
    public interface IFeedsConvertor
    {
        public Task<string> GetFeeds();
    }
}
