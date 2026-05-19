using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        // POST: api/customers/register
        [HttpPost("register")]
        public async Task<ActionResult<Customer>> RegisterCustomer(Customer customer)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                return BadRequest("A user with this email already exists.");
            }

            // Hash the password securely
            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(customer.PasswordHash);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful!", CustomerID = customer.CustomerID });
        }

        // POST: api/customers/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request, [FromServices] IConfiguration config)
        {
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Email == request.Email);

            // Verify email and hashed password
            if (customer == null || !BCrypt.Net.BCrypt.Verify(request.Password, customer.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerID),
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim(ClaimTypes.Name, customer.CustomerName)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Message = "Login successful",
                Token = tokenString,
                CustomerID = customer.CustomerID,
                Name = customer.CustomerName,
                SavedAddresses = customer.Addresses
            });
        }

        // GET: api/customers (Optional: You can lock this down by adding [Authorize] above it)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.Include(c => c.Addresses).ToListAsync();
        }
    }
}