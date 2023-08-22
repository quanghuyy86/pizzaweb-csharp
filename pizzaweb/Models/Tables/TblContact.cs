using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblContact
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
