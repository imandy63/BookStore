using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BookStore.ViewModel
{
    public class ProfileModel
    {
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [RegularExpression(@"^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ ]*$", ErrorMessage = "Tên không chứa ký tự đặc biệt")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại")]
        [MinLength(10, ErrorMessage = "Vui lòng nhập đầy đủ số điện thoại (Điện thoại Việt Nam, 10 chữ số)")]
        [RegularExpression(@"^(0)(3[2-9]|5[2689]|7[06789]|8[1-689]|9[0-9])[0-9]{7}$", ErrorMessage = "Vui lòng nhập đúng số điện thoại Việt Nam")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
    }
}