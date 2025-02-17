﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities
{
    public class FavoriteList : BaseEntity
    {
        public string UserId { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }

        // Navigation properties
        public virtual ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
    }
}
