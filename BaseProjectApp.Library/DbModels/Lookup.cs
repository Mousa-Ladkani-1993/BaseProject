using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class Lookup
    {
        public Lookup()
        {
            LookupValues = new HashSet<LookupValue>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? CanChange { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<LookupValue> LookupValues { get; set; }
    }
}
