using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{ 

    
    public class CompanyLookupValueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PagingDto
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        
        public int Total { get; set; }
    }
 
     
}
