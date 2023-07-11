using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
    public class NotificationTemplateResponse
    {
        public int Id { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }
        public string? TextEn { get; set; }
        public string? TextAr { get; set; }
        public string? Screen { get; set; } 
        public string? DescriptionEn { get; set; }
        public string? NameEn { get; set; }
    }

 
}
