using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblProduct
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
        public string DetailDescription { get; set; } = null!;
        public string? Avatar { get; set; }
        public int? CategoryId { get; set; }

        public virtual TblCategory? Category { get; set; }
    }
}
