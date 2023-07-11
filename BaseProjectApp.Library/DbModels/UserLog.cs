using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class UserLog
    {
        public int Id { get; set; }
        public int? RecordId { get; set; }
        public string? TableName { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? UserId { get; set; }

        public virtual AspNetUser? User { get; set; }
    }
}
