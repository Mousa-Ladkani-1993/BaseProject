using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class CompanyLookupValue
    { 
        public int Id { get; set; }
        public string? ValueAr { get; set; }
        public string? ValueEn { get; set; }
        public int? CompanyLookupId { get; set; }
        public bool CanChange { get; set; }
        public int? OrderNb { get; set; }
        public int Visible { get; set; }
        public bool Deleted { get; set; }

        public virtual CompanyLookup? CompanyLookup { get; set; } 
    }
}
