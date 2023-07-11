using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class LookupValue
    { 

        public int Id { get; set; }
        public string? ValueEn { get; set; }
        public string? ValueAr { get; set; }
        public int? LookupId { get; set; }
        public bool? CanChange { get; set; }
        public int? OrderNb { get; set; }
        public int Visible { get; set; }
        public bool Deleted { get; set; }

        public virtual Lookup? Lookup { get; set; } 
    }
}
