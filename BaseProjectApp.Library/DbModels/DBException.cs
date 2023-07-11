using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class Dbexception
    {
        public int ExceptionId { get; set; }
        public string? Subject { get; set; }
        public string? Details { get; set; }
        public DateTime? ExceptionDate { get; set; }
    }
}
