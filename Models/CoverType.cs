using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class CoverType
    {
        [Key]
        public int CoverTypeId { get; set; }

        [Display(Name = "Cover Type Name")]
        [Required]
        [MaxLength(50)]
        public string CoverTypeName { get; set; }
    }
}
