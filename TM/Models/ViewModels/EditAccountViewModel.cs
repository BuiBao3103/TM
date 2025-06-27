using System.ComponentModel.DataAnnotations;

namespace TM.Models.ViewModels
{
    public class EditAccountViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống.")]
        public string Role { get; set; }

        public string? OldPassword { get; set; }

        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        public string? NewPassword { get; set; }
        
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }
    }
} 