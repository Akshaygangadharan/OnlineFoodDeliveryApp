using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models
{
    public class Customer
    {
        [Key]
        public string CustomerID { get; set; } = Guid.NewGuid().ToString(); // PK as string per your diagram

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property for the One-to-Many relationship
        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
    }
}