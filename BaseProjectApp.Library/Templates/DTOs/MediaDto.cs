using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectApp.Library.Templates.DTOs
{
    public class MediaDto
    {
        public string Table { get; set; }
        public string CaptionEn { get; set; }
        public string CaptionAr { get; set; }
        public string YoutubePath { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? MainImage { get; set; }
        public int MediaType { get; set; }
        public string ListOfIds { get; set; }
        public int RecordId { get; set; }
    }
      

    public class _MediaFileShortcut
    {
        public int Id { get; set; }
        public string PlatformPath { get; set; }
        public bool? MainImage { get; set; }
        public int? MediaType { get; set; }
        public string FilePath { get; set; }
        public string Caption { get; set; }
    }
}
