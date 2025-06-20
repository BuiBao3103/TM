using System;
using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class PassengerEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã khách là bắt buộc")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Số CMND/CCCD là bắt buộc")]
        [StringLength(20)]
        public string IdentityNumber { get; set; }

        [StringLength(20)]
        public string? PassportNum { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(30)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AssignedPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CustomerPaid { get; set; }

        [StringLength(100)]
        public string? DepartureFlightInfo { get; set; }

        [StringLength(100)]
        public string? ArrivalFlightInfo { get; set; }

        [Required]
        public string Status { get; set; }
    }
} 