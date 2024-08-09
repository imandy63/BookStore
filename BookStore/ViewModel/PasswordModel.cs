using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class PasswordModel
    {
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Không khớp")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}