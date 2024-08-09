using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Tên tác giả")]
        public string AuthorName { get; set; }
        [Display(Name = "Giá")]
        [Required(ErrorMessage="Vui lòng nhập giá")]
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageURL { get; set; }
        [Display(Name = "Số lượng")]
        [Range(0,10000,ErrorMessage="Nhập số lượng từ 0 - 10000")]
        [Required(ErrorMessage="Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
        [Required(ErrorMessage="Vui lòng chọn thể loại sách")]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}