using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class FeedItem
{
    public string Id { get; set; } = null!;

    public string FeedId { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public virtual Rssfeed Feed { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
