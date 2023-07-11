using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class MobileCustomMenu
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? Summary { get; set; }
        public string? SummaryAr { get; set; }
        public string? Details { get; set; }
        public string? DetailsAr { get; set; }
        public bool? Active { get; set; }
        public int? Priority { get; set; }
        public int? ParentId { get; set; }
        public string? Link { get; set; }
        public bool? ShowInDrawer { get; set; }
        public string? IconUrl { get; set; }
        public bool Deleted { get; set; }
    }
}
