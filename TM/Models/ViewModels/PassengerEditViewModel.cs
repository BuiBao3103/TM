using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class PassengerEditViewModel
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
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [Display(Name = "Giới tính")]
        [GenderValidation(ErrorMessage = "Giới tính không hợp lệ")]
        public string Gender { get; set; }

       
        [Display(Name = "Số CMND/CCCD")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số CCCD chỉ được chứa chữ số")]
        [StringLength(20, ErrorMessage = "Số CCCD không được vượt quá 20 ký tự")]
        public string? IdentityNumber { get; set; }

        
        [Display(Name = "Số hộ chiếu")]
        [StringLength(9, MinimumLength = 6, ErrorMessage = "Số hộ chiếu phải từ 6 đến 9 ký tự.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Số hộ chiếu chỉ được chứa chữ và số.")]

        public string? PassportNum { get; set; }

        [Display(Name = "Ngày hết hạn hộ chiếu")]
        public DateOnly? PassportExpiryDate { get; set; }

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

        [Display(Name = "Ghi chú")]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Tour là bắt buộc")]
        public int TourId { get; set; }

        [Required(ErrorMessage = "Giá đề xuất là bắt buộc")]
        [Display(Name = "Giá đề xuất")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal AssignedPrice { get; set; }

        [Required(ErrorMessage = "Số tiền giảm là bắt buộc")]
        [Display(Name = "Số tiền giảm")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = "Phí hoa hồng là bắt buộc")]
        [Display(Name = "Phí hoa hồng")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal? hhFee { get; set; }

        [Required(ErrorMessage = "Số tiền khách trả là bắt buộc")]
        [Display(Name = "Khách đã trả")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        [CustomerPaidNotGreaterThanAssignedPrice("AssignedPrice", ErrorMessage = "Số tiền khách trả không được lớn hơn giá đã đề xuất")]
        public decimal CustomerPaid { get; set; }

        [StringLength(100, ErrorMessage = "Thông tin không được vượt quá 100 ký tự")]
        public string? DepartureFlightInfo { get; set; }

        [StringLength(100, ErrorMessage = "Thông tin không được vượt quá 100 ký tự")]
        public string? ArrivalFlightInfo { get; set; }

        [Required(ErrorMessage = "Trạng thái khách hàng không được thiếu")]
        [Display(Name = "Trạng thái")]
        [StatusValidation(ErrorMessage = "Trạng thái không hợp lệ")]
        public string Status { get; set; }
    }
}