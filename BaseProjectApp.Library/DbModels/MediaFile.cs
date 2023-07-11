using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class MediaFile
    {
        public int Id { get; set; }
        public string? CaptionEn { get; set; }
        public string CaptionAr { get; set; } = null!;
        public bool? MainImage { get; set; }
        public int? TypeId { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? YouTubePath { get; set; }
        public int? RecordId { get; set; }
        public int? TopicId { get; set; }
        public int? UserInfoId { get; set; }
        public bool Deleted { get; set; }
        public int? ChildId { get; set; }
        public int? ClientId { get; set; }
        public int? SliderId { get; set; }
        public int? EventId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductVariationId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? SizeChartId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductAttributeValueId { get; set; }
        public int? PrimaryMedia { get; set; }
        public int? CommunityId { get; set; }
        public int? CommunityPostId { get; set; }
        public int? ExpertId { get; set; }
        public string? UserProfileId { get; set; }
        public bool? IsHorizontal { get; set; }
        public int? RecordCommentId { get; set; }
        public int? SupplierId { get; set; }
        public int? SupplierDocumentId { get; set; }
        public int? SupplierLogo { get; set; }
        public bool? IsIcon { get; set; }
        public int? EventCommentId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectTimelineId { get; set; }
        public int? PartnerDonorId { get; set; }
        public int? JobVacancyId { get; set; }
        public int? TeamMemberId { get; set; }
        public int? ApplicantId { get; set; }
        public int? CareerId { get; set; }
        public int? PublicationId { get; set; }
        public int? CountryId { get; set; }
        public string? LanguageId { get; set; }
        public int? PropertyId { get; set; }
    }
}
