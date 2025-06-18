using System;
using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class TourInfoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên tour là bắt buộc")]
        [Display(Name = "Tên tour")]
        [StringLength(200, ErrorMessage = "Tên tour không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mã tour là bắt buộc")]
        [Display(Name = "Mã tour")]
        [StringLength(50, ErrorMessage = "Mã tour không được vượt quá 50 ký tự")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Tổng số chỗ là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Tổng số chỗ phải lớn hơn 0")]
        [Display(Name = "Tổng số chỗ")]
        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        [Required(ErrorMessage = "Giá gợi ý là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        [Display(Name = "Giá gợi ý")]
        public decimal SuggestPrice { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải là số dương")]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = "Phí hoa hồng là bắt buộc")]
        [Display(Name = "Phí hoa hồng")]
        [Range(0, double.MaxValue, ErrorMessage = "Phí hoa hồng phải là số dương")]
        public decimal HhFee { get; set; }

        [Required(ErrorMessage = "Chuyến bay đi là bắt buộc")]
        [Display(Name = "Chuyến bay đi")]
        public string? DepartureFlightInfo { get; set; }

        [Required(ErrorMessage = "Chuyến bay về là bắt buộc")]
        [Display(Name = "Chuyến bay về")]
        public string? ArrivalFlightInfo { get; set; }


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

    }
}
