using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Item
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string? Description { get; set; }

    public string? Author { get; set; }

    public DateOnly? Date { get; set; }

    public virtual ICollection<FeedItem> FeedItems { get; } = new List<FeedItem>();
    public virtual ICollection<WatchedRss> WatchedRsss { get; } = new List<WatchedRss>();
}
