using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class TourEditViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên tour không được để trống.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã tour không được để trống.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Tổng chỗ phải lớn hơn 0.")]
        public int TotalSeats { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Chỗ trống không hợp lệ.")]
        public int AvailableSeats { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá đề xuất không hợp lệ.")]
        public decimal SuggestPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá giảm không hợp lệ.")]
        public decimal? DiscountPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Phí HH không hợp lệ.")]
        public decimal HhFee { get; set; }

        public string? DepartureFlightInfo { get; set; }
        public string? ArrivalFlightInfo { get; set; }
        public bool? IsAutoHoldTime { get; set; }
        public int? HoldTime { get; set; }
        public bool? IsVisaRequired { get; set; }
        public DateTime? VisaDeadline { get; set; }
        public DateTime? FullPayDeadline { get; set; }
        public int? LocationId { get; set; }
        public string? DepartureLocation { get; set; }
        public string? RoomNote { get; set; }
        public string? Note { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

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