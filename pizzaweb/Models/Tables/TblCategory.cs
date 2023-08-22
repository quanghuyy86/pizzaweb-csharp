using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
