using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
    public class CompanyLookupInnerDTO
    {
        public int Id { get; set; }
        public string? ValueEn { get; set; }
        public string? ValueAr { get; set; }
        public int? CompanyLookupId { get; set; }

        public string? CompanyLookupName { get; set; } = null;
    }



    public class LookupInnerDTO
    {
        public int Id { get; set; }
        public string? ValueEn { get; set; }
        public string? ValueAr { get; set; }
        public int? LookupId { get; set; }

        public string? LookupName { get; set; } = null;
    }
}
