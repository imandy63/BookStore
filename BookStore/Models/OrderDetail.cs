using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}