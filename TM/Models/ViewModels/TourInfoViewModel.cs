using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class TourInfoViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Tên tour là bắt buộc")]
        [Display(Name = "Tên tour")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Mã tour là bắt buộc")]
        [Display(Name = "Mã tour")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public required DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc")]
        public required DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Tổng số chỗ là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Tổng số chỗ phải lớn hơn 0")]
        [Display(Name = "Tổng số chỗ")]
        public required int TotalSeats { get; set; }

        public int? AvailableSeats { get; set; }

        [Required(ErrorMessage = "Giá gợi ý là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá gợi ý")]
        public required decimal SuggestPrice { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải là số dương")]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = "Phí hoa hồng là bắt buộc")]
        [Display(Name = "Phí hoa hồng")]
        [Range(0, double.MaxValue, ErrorMessage = "Phí hoa hồng phải là số dương")]
        public required decimal HhFee { get; set; }

        [Required(ErrorMessage = "Chuyến bay đi là bắt buộc")]
        [Display(Name = "Chuyến bay đi")]
        public required string DepartureFlightInfo { get; set; }

        [Required(ErrorMessage = "Chuyến bay về là bắt buộc")]
        [Display(Name = "Chuyến bay về")]
        public required string ArrivalFlightInfo { get; set; }

        [Display(Name = "Giữ chỗ tự động")]
        public bool? IsAutoHoldTime { get; set; }

        [Display(Name = "Thời gian giữ chỗ (phút)")]
        [Range(0, int.MaxValue, ErrorMessage = "Thời gian giữ chỗ phải là số không âm")]
        public int? HoldTime { get; set; }

        [Display(Name = "Cần visa")]
        public bool? IsVisaRequired { get; set; }

        [Display(Name = "Hạn visa")]
        [DataType(DataType.Date)]
        public DateTime? VisaDeadline { get; set; }

        [Display(Name = "Hạn thanh toán toàn phần")]
        [DataType(DataType.Date)]
        public DateTime? FullPayDeadline { get; set; }

        [Required(ErrorMessage = "Mã địa điểm là bắt buộc")]
        [Display(Name = "Mã địa điểm")]
        public int? LocationId { get; set; }

        [Required(ErrorMessage = "Nơi khởi hành là bắt buộc")]
        [Display(Name = "Nơi khởi hành")]
        public string? DepartureLocation { get; set; }

        [Display(Name = "Ghi chú phòng")]
        public string? RoomNote { get; set; }

        [Display(Name = "Ghi chú khác")]
        public string? Note { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Custom validation method
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Ngày bắt đầu < ngày kết thúc
            if (StartDate >= EndDate)
            {
                results.Add(new ValidationResult(
                    "Ngày bắt đầu phải nhỏ hơn ngày kết thúc",
                    new[] { nameof(StartDate), nameof(EndDate) }));
            }

            // Ngày bắt đầu phải từ hôm nay trở đi
            if (StartDate.Date < DateTime.Today)
            {
                results.Add(new ValidationResult(
                    "Ngày bắt đầu không được là ngày trong quá khứ",
                    new[] { nameof(StartDate) }));
            }

            // Giá khuyến mãi < giá gợi ý 
            if (DiscountPrice.HasValue && DiscountPrice >= SuggestPrice)
            {
                results.Add(new ValidationResult(
                    "Giá khuyến mãi phải nhỏ hơn giá gợi ý",
                    new[] { nameof(DiscountPrice) }));
            }

            // Phí hoa hồng < giá gợi ý
            if (HhFee >= SuggestPrice)
            {
                results.Add(new ValidationResult(
                    "Phí hoa hồng phải nhỏ hơn giá gợi ý",
                    new[] { nameof(HhFee) }));
            }

            // Hạn visa phải > ngày bắt đầu (nếu cần visa)
            if (IsVisaRequired == true && VisaDeadline.HasValue && VisaDeadline >= StartDate)
            {
                results.Add(new ValidationResult(
                    "Hạn visa phải trước ngày bắt đầu tour",
                    new[] { nameof(VisaDeadline) }));
            }

            // Hạn thanh toán phải trước ngày bắt đầu
            if (FullPayDeadline.HasValue && FullPayDeadline >= StartDate)
            {
                results.Add(new ValidationResult(
                    "Hạn thanh toán phải trước ngày bắt đầu tour",
                    new[] { nameof(FullPayDeadline) }));
            }

            return results;
        }
    }
}