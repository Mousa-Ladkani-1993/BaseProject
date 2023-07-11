using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{
    public class _LocationDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? ISOCode3 { get; set; }
    }

    public class LocationDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public string? ISOCode3 { get; set; }
    }

    public class LocationNodeDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public int? TypeId { get; set; }
        public string? ISOCode3 { get; set; }
        public List<LocationNodeDTO> Children { get; set; }
    }

}
