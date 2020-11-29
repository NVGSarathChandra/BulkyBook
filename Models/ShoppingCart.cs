using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }


        [Key]
        public int ShoppingCartId { get; set; }

        public string ApplicatinUserId { get; set; }

        [ForeignKey("ApplicatinUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Count of properties should be betweenn 1 to 1000")]
        public int Count { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
