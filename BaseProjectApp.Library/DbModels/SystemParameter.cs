using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class SystemParameter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TextValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public bool? BoolValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? Editable { get; set; }
        public int? Type { get; set; }
    }
}
