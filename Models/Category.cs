using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name ="Category Name")]
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
    }
}
