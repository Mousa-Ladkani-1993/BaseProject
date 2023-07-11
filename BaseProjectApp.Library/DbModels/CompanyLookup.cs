using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class CompanyLookup
    {
        public CompanyLookup()
        {
            CompanyLookupValues = new HashSet<CompanyLookupValue>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool CanChange { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<CompanyLookupValue> CompanyLookupValues { get; set; }
    }
}
