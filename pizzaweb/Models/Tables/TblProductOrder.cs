using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblProductOrder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }

        public virtual TblOrder Order { get; set; } = null!;
    }
}
