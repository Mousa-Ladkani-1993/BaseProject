using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class ViewCity
    {
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public int? ParentId { get; set; }
    }
}
