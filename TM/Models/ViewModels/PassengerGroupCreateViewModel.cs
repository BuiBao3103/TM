using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels;

public class PassengerGroupCreateViewModel
{
    [Required(ErrorMessage = "Tên đoàn là bắt buộc")]
    [Display(Name = "Tên đoàn")]
    [StringLength(100, ErrorMessage = "Tên đoàn không được vượt quá 100 ký tự")]
    public string GroupName { get; set; } = null!;

    [Required(ErrorMessage = "Số lượng thành viên là bắt buộc")]
    [Range(1, 100, ErrorMessage = "Số lượng thành viên phải từ 1 đến 100")]
    [Display(Name = "Số lượng thành viên")]
    public int TotalMember { get; set; }

    [Display(Name = "Ghi chú")]
    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string? Note { get; set; }

    // Thông tin người đại diện
    [Required(ErrorMessage = "Mã khách đại diện là bắt buộc")]
    [Display(Name = "Mã khách")]
    [StringLength(50, ErrorMessage = "Mã khách không được vượt quá 50 ký tự")]
    public string RepresentativeCode { get; set; } = null!;

    [Required(ErrorMessage = "Họ tên người đại diện là bắt buộc")]
    [Display(Name = "Họ tên")]
    [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
    public string RepresentativeName { get; set; } = null!;

    [Display(Name = "Số điện thoại")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số")]
    [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
    public string? RepresentativePhone { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(30, ErrorMessage = "Email không được vượt quá 30 ký tự")]
    public string? RepresentativeEmail { get; set; }

    [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateTime RepresentativeBirthDate { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    [Display(Name = "Giới tính")]
    [GenderValidation(ErrorMessage = "Giới tính không hợp lệ")]
    public string RepresentativeGender { get; set; } = null!;

    [Display(Name = "Số CMND/CCCD")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Số CCCD chỉ được chứa chữ số")]
    [StringLength(20, ErrorMessage = "Số CCCD không được vượt quá 20 ký tự")]
    public string? RepresentativeIdentityNumber { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
    public string? RepresentativeAddress { get; set; }

    // Thông tin thanh toán
    [Required(ErrorMessage = "Giá đề xuất là bắt buộc")]
    [Display(Name = "Giá đề xuất")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal? AssignedPrice { get; set; }

    [Display(Name = "Số tiền giảm")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal? DiscountPrice { get; set; }

    [Display(Name = "Phí hoa hồng")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal? HhFee { get; set; }

    [Display(Name = "Khách đã trả")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    [CustomerPaidNotGreaterThanAssignedPrice("AssignedPrice", ErrorMessage = "Số tiền khách trả không được lớn hơn giá đã đề xuất")]
    public decimal? CustomerPaid { get; set; }

    [Required(ErrorMessage = "Trạng thái khách hàng không được thiếu")]
    [Display(Name = "Trạng thái")]
    [StatusValidation(ErrorMessage = "Trạng thái không hợp lệ")]
    public string Status { get; set; } = "Reserved";

    // Hidden field để lưu TourId
    public int TourId { get; set; }
} 