using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TM.Models.ViewModels
{

    public enum PassengerGender
    {
        [Display(Name = "Nam")]
        Male,

        [Display(Name = "Nữ")]
        Female,

        [Display(Name = "Khác")]
        Other
    }

    public enum PassengerStatus
    {
        Reserved,
        Confirm,
        Paid
        Cancel
    }

    public class TourPassengerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã khách là bắt buộc")]
        [Display(Name = "Mã khách")]
        [StringLength(50, ErrorMessage = "Mã khách không được vượt quá 50 ký tự")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [Display(Name = "Giới tính")]
        [GenderValidation(ErrorMessage = "Giới tính không hợp lệ")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Số CMND/CCCD là bắt buộc")]
        [Display(Name = "Số CMND/CCCD")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số CCCD chỉ được chứa chữ số")]
        [StringLength(20, ErrorMessage = "Số CCCD không được vượt quá 20 ký tự")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string? Phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(30, ErrorMessage = "Email không được vượt quá 30 ký tự")]
        public string? Email { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Tour là bắt buộc")]
        public int TourId { get; set; }

        [Display(Name = "Tên Tour")]
        public string TourName { get; set; }

        [Display(Name = "Mã Tour")]
        public string TourCode { get; set; }

        [Required(ErrorMessage = "Giá chào là bắt buộc")]
        [Display(Name = "Giá chào")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal AssignedPrice { get; set; }

        [Required(ErrorMessage = "Số tiền khách trả là bắt buộc")]
        [Display(Name = "Khách đã trả")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal CustomerPaid { get; set; }

        [Required(ErrorMessage = "Trạng thái khách hàng không được thiếu")]
        [Display(Name = "Trạng thái")]
        [StatusValidation(ErrorMessage = "Trạng thái không hợp lệ")]
        public string Status { get; set; }
    }

    /// Custom validation attribute to check gender enum values
    public class GenderValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            return Enum.TryParse(typeof(PassengerGender), value.ToString(), ignoreCase: true, out _);
        }
    }

    public class StatusValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            return Enum.TryParse(typeof(PassengerStatus), value.ToString(), ignoreCase: true, out _);
        }
    }
}
