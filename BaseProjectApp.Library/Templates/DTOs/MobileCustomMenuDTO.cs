using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{
    public class MobileCustomMenuDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? Priority { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; } 
        public string? Summary { get; set; } 
        public string? Details { get; set; }
        public string? Link { get; set; }
        public string? IconUrl { get; set; }
        public bool? ShowInDrawer { get; set; }
    }

    public class MobileCustomMenuNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? Priority { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? Summary { get; set; }
        public string? Details { get; set; }
        public string? Link { get; set; }
        public string? IconUrl { get; set; }
        public bool? ShowInDrawer { get; set; }
        public List<MobileCustomMenuNode> Children { get; set; }
    }
}
