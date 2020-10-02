using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniCrm.DataModel
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(10)]
        public string PhoneExtension { get; set; }
        [StringLength(100)]
        public string AddressLine1 { get; set; }
        [StringLength(100)]
        public string AddressLine2 { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
    }
}
