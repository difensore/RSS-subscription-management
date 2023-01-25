using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Rssfeed
{
    public string Id { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Title { get; set; }

    public virtual ICollection<FeedItem> FeedItems { get; } = new List<FeedItem>();
    public virtual ICollection<WatchedRss> WatchedRsss { get; } = new List<WatchedRss>();
}
