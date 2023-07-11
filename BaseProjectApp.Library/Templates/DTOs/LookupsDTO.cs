using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{

    public class LookUpValueDTO
    {
        public int Id { get; set; }
        public string? Value { get; set; }

    }
    public class CompanyLookupValueDTO
    {
        public int Id { get; set; }
        public string? Value { get; set; } 
        public int? CompanyLookupId { get; set; }
        public string?  CompanyLookupName { get; set; }
    }



    public class LookupValueDTO
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public int? LookupId { get; set; }
        public string? LookupName { get; set; }
    }
}
