using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerDbContext _context;

        public CustomersController(CustomerDbContext context)
        {
            _context = context;
        }

        // 1. REGISTRATION (Create a new customer securely)
        // POST: api/customers/register
        [HttpPost("register")]
        public async Task<ActionResult<Customer>> RegisterCustomer(Customer customer)
        {
            // Check if the email is already taken
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                return BadRequest("A user with this email already exists.");
            }

            // HASH THE PASSWORD before saving it to the database
            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(customer.PasswordHash);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful!", CustomerID = customer.CustomerID });
        }

        // 2. LOGIN (Authenticate the customer and fetch their addresses)
        // POST: api/customers/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // Find the user by email AND bring their addresses along with them
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Email == request.Email);

            // If no user found, or if the password doesn't match the hash
            if (customer == null || !BCrypt.Net.BCrypt.Verify(request.Password, customer.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // SUCCESS! Return the customer details and their saved delivery addresses
            return Ok(new
            {
                Message = "Login successful",
                CustomerID = customer.CustomerID,
                Name = customer.CustomerName,
                SavedAddresses = customer.Addresses
            });
        }
    }
}