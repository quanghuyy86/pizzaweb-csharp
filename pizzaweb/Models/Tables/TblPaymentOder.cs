using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblPaymentOder
    {
        public TblPaymentOder()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
