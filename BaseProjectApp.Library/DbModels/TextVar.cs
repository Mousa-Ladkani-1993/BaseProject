using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class TextVar
    {
        public int Id { get; set; }
        public string? DataAr { get; set; }
        public string? DataEn { get; set; }
        public string? LinkAr { get; set; }
        public string? LinkEn { get; set; }
        public string? TextKey { get; set; }
        public string? Name { get; set; }
    }
}
