using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels;

public class TourSurchargeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phụ phí là bắt buộc")]
    [Display(Name = "Tên phụ phí")]
    [StringLength(100, ErrorMessage = "Tên phụ phí không được vượt quá 100 ký tự")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Số tiền là bắt buộc")]
    [Display(Name = "Số tiền")]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Tour là bắt buộc")]
    public int TourId { get; set; }

    [Display(Name = "Tour")]
    public string? TourName { get; set; }
}