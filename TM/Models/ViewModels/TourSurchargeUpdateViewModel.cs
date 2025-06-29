
using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class TourSurchargeUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phụ phí là bắt buộc")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        public required decimal? Amount { get; set; }
    }
}
