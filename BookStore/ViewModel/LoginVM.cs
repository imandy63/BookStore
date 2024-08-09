using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "Tên tài khoản không chứa khoản trắng và ký tự đặc biệt")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}