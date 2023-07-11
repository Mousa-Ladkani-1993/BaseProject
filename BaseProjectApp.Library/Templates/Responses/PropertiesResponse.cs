using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{ 

    public class PropertyResponse
    {
        public int Id { get; set; } 
        public string? Label { get; set; } 
        public string? Name { get; set; }
        public string? BLabel { get; set; } 
        public string? Type { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AreaLocation { get; set; }
        public decimal? Area { get; set; }
        public string? Location { get; set; }
        public decimal? Price { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfParkingSpaces { get; set; }
        public int? Floor { get; set; }
    }


    public class SearchResData
    {
        public List<PropertySpRes>? Data { get; set; }
        public List<PropertySpResCount> CountObj { get; set; }
    }

    public class PropertySpResCount
    {
        public int? Total { get; set; }

    }

    public class PropertySpRes
    {
        public int? CurrencyId { get; set; }
        public int Id { get; set; }
        public int? AreaMeasurementId { get; set; }
        public string? ImgUrl { get; set; }
        public string? Name { get; set; }
        public string? _Type { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AreaName { get; set; }
        public decimal? Area { get; set; }
        public decimal? Price { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfParkingSpaces { get; set; }
        public int? Floor { get; set; }
        public int? NumberOfViews { get; set; }
        public int? NumberOfLikes { get; set; }
        public bool? Favorite { get; set; }
        public bool? Featured { get; set; }
        public bool? Premium { get; set; }
        public bool? Published { get; set; }
        public bool? Approved { get; set; }
        public string? Pricelbl { get; set; } 
        public string? Arealbl { get; set; }
        public DateTime? PublishDate { get; set; }
        public decimal? RatingValue { get; set; }
    }



    public class PropertySpRes_lbl
    {
        public int? CurrencyId { get; set; }
        public int Id { get; set; }
        public int? AreaMeasurementId { get; set; }
        public string? ImgUrl { get; set; }
        public string? Name { get; set; }
        public string? _Type { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AreaName { get; set; }
        public decimal? Area { get; set; }
        public decimal? Price { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfParkingSpaces { get; set; }
        public int? Floor { get; set; }
        public int? NumberOfViews { get; set; }
        public int? NumberOfLikes { get; set; }
        public bool? Favorite { get; set; }
        public bool? Featured { get; set; }
        public bool? Premium { get; set; }
        public bool? Published { get; set; }
        public bool? Approved { get; set; }
        public string? Pricelbl { get; set; }
        public string? Arealbl { get; set; }
        public DateTime? PublishDate { get; set; }
        public decimal? RatingValue { get; set; }
    }
}
