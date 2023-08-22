using System;
using System.Collections.Generic;

namespace pizzaweb.Models.Tables
{
    public partial class TblUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public bool? Status { get; set; }
    }
}
