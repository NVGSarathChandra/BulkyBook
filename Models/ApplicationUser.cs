using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        //[Required(ErrorMessage ="Username is required")]
        //public override string UserName { get => base.UserName; set => base.UserName = value; }

        public int? OrganizatioinId { get; set; }
        [ForeignKey("OrganizatioinId")]
        public Organization Organization { get; set; }


        [NotMapped]
        public string Role { get; set; }
    }
}
