using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblCustomer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
