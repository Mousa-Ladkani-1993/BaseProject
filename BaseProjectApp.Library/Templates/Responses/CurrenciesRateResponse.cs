using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
     

    public class CurrenciesRateResponse
    {
        public int Id { get; set; }
        public int? FromCurrencyId { get; set; }
        public int? ToCurrencyId { get; set; }
        public decimal? Rate { get; set; }
        public string? RateSTR { get; set; }
        public decimal? ReverseRate { get; set; } 
        public string? FromCurrencyName { get; set; }
        public string? ToCurrencyName { get; set; }
    }

    public class CurrenciesResponse
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public string? ValueEn { get; set; }
        public string? ValueAr { get; set; }
        public bool? Published { get; set; }
        public int? DisplayOrder { get; set; } 
    }
}
