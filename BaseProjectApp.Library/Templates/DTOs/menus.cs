using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{

    public class MenuDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? PageUrl { get; set; }
        public bool? InNewWindow { get; set; }
        public bool? Published { get; set; }
        public int? InDynamicPage { get; set; }
        public string? MetaTag { get; set; }
        public int? Priority { get; set; }
        public List<MenuDto> SubMenus { get; set; }
    }


    public class HomeMenuDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? PageUrl { get; set; }
        public bool? InNewWindow { get; set; }
        public int? InDynamicPage { get; set; }
        public string? MetaTag { get; set; }
        public int? Priority { get; set; }
        public List<HomeMenuDto> SubMenus { get; set; }
    } 
}
