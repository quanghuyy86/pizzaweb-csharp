using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblProductOrders = new HashSet<TblProductOrder>();
        }

        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CutomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Payment { get; set; }

        public virtual TblPaymentOder PaymentNavigation { get; set; } = null!;
        public virtual ICollection<TblProductOrder> TblProductOrders { get; set; }
    }
}
