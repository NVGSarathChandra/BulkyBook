using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;

namespace Models
{
    public class Organization
    {
       [Key]
        public int OrganizationId { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAuthorizedOrganization { get; set; }

    }
}
