using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }

        [Required]
        public string ProductISBN { get; set; }
        public string ProductAuthor { get; set; }

        [Required]
        [Range(1, 100000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 100000)]
        public double Price { get; set; }

        [Required]
        [Range(1, 100000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 100000)]
        public double Price100 { get; set; }

        public string ImageURL { get; set; }

        #region Foriegn Key reference for category ID

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category category { get; set; }

        #endregion

        #region Foriegn Key reference for Cover Type ID

        [Required]
        public int CoverTypeId { get; set; }

        [ForeignKey("CoverTypeId")]
        public CoverType coverType { get; set; }

        #endregion

    }
}
