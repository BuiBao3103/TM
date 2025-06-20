using System;
using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class TourEditViewModel
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
    }
} 