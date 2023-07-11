using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{

    public class QuickSearchFilterRes
    {
        public int? SelectedAreaUnitId { get; set; } = 0;
        public int? SelectedCurrencyId { get; set; } = 0;


        public string? SearchTerm { get; set; } = "";
        public int? CountryId { get; set; } = 0;
        public int? CityId { get; set; } = 0;
        public int? AreaId { get; set; } = 0;


        public double? PriceFrom { get; set; } = 0;
        public double? PriceTo { get; set; } = 0;

        public double? AreaFrom { get; set; } = 0;
        public double? AreaTo { get; set; } = 0;

        public int? TypeId { get; set; } = 0;
        public int? StatusId { get; set; } = 0;
        public int? BusinessTypeId { get; set; } = 0;


        public int? AgentId { get; set; } = 0;
        public int? NumberOfBedrooms { get; set; } = 0;
        public int? NumberOfParkingSpaces { get; set; } = 0;
        public int? NumberOfBathrooms { get; set; } = 0;
        public int? PaymentTypeId { get; set; } = 0;
        public List<int>? Amenities { get; set; }
        public int? funishedId { get; set; } = 0;
        public int? OwnershipId { get; set; } = 0;
        public int? Floor { get; set; } = 0;
         
        public bool? Premium { get; set; }
        public bool? Published { get; set; }
        public bool? Featured { get; set; }

        public bool? PendingProps { get; set; }
        
        public int? NumberofLikes { get; set; }
        public int? NumberofViews { get; set; }
        public double? MonthlyFee { get; set; }
        public int? YearBuilt { get; set; } 
        public DateTime?  EndPublishDate { get; set; }
        public DateTime? StartPublishDate { get; set; }
        public ParameterPagination Pager { get; set; }

    }

    public class PropertiesTypeRes
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? ValueEn { get; set; }
        public string? ValueAr { get; set; }
        public string? ImgUrl { get; set; }
        public bool? Published { get; set; }
        public int? DisplayOrder { get; set; }

    }
}
