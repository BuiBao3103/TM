using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels;

public class PassengerViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Mã khách là bắt buộc")]
    [Display(Name = "Mã khách")]
    [StringLength(50)]
    public string? Code { get; set; }

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [Display(Name = "Họ tên")]
    [StringLength(100)]
    public string? FullName { get; set; }

    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    [Display(Name = "Giới tính")]
    public string? Gender { get; set; }

    [Display(Name = "Số CMND/CCCD")]
    [StringLength(20)]
    public string? IdentityNumber { get; set; }

    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(15)]
    public string? Phone { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Tour là bắt buộc")]
    [Display(Name = "Mã Tour")]
    public int? TourId { get; set; }

    [Required(ErrorMessage = "Giá chào là bắt buộc")]
    [Display(Name = "Giá chào")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal? AssignedPrice { get; set; }

    [Required(ErrorMessage = "Số tiền khách trả là bắt buộc")]
    [Display(Name = "Khách đã trả")]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải >= 0")]
    public decimal? CustomerPaid { get; set; }

    [Display(Name = "Số tiền giảm")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal DiscountPrice { get; set; }

    [Display(Name = "Phí hoa hồng")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal hhFee { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    [Display(Name = "Trạng thái")]
    public string Status { get; set; }

    // Optional: Tên tour cho hiển thị (nếu cần)
    [Display(Name = "Tên Tour")]
    public string? TourName { get; set; }

    public string? PassportNum { get; set; }

    [Display(Name = "Ngày hết hạn hộ chiếu")]
    public DateOnly? PassportExpiryDate { get; set; }

    public string? DepartureFlightInfo { get; set; }

    public string? ArrivalFlightInfo { get; set; }

    public int? ModifiedById { get; set; }

}