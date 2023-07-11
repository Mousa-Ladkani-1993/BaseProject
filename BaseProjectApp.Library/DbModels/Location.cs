using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class Location
    { 
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public string? Isocode3 { get; set; } 
    }
}
