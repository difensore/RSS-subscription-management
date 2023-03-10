using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class WatchedRss
    {
        public string Id { get; set; } = null!;

        public string ItemId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public virtual Item Item { get; set; } = null!;

        public virtual IdentityUser User { get; set; } = null!;
    }
}
