using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.Models
{
    public class CustomerAddress
    {
        [Key]
        public string CustomerAddressID { get; set; } = Guid.NewGuid().ToString(); // PK

        [Required]
        [ForeignKey("Customer")]
        public string CustomerID { get; set; } // FK to Customer

        public string ReceiverName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        // Navigation property mapping back to the Customer
        public Customer Customer { get; set; }
    }
}