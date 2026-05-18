using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Controllers
{
    [Authorize] // This requires a valid JWT Token for ALL actions in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly CustomerDbContext _context;

        public CustomerAddressesController(CustomerDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerAddresses
        // Retrieves all addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetCustomerAddresses()
        {
            return await _context.CustomerAddresses.ToListAsync();
        }

        // GET: api/CustomerAddresses/5
        // Retrieves a specific address by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetCustomerAddress(string id)
        {
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);

            if (customerAddress == null)
            {
                return NotFound("Address not found.");
            }

            return customerAddress;
        }

        // POST: api/CustomerAddresses
        // Creates a new address
        [HttpPost]
        public async Task<ActionResult<CustomerAddress>> PostCustomerAddress(CustomerAddress customerAddress)
        {
            _context.CustomerAddresses.Add(customerAddress);
            await _context.SaveChangesAsync();

            // Returns a 201 Created status and points to the GET method to fetch the new resource
            return CreatedAtAction(nameof(GetCustomerAddress), new { id = customerAddress.CustomerAddressID }, customerAddress);
        }

        // PUT: api/CustomerAddresses/5
        // Updates an existing address
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAddress(string id, CustomerAddress customerAddress)
        {
            if (id != customerAddress.CustomerAddressID)
            {
                return BadRequest("The ID in the URL does not match the ID in the body.");
            }

            _context.Entry(customerAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(id))
                {
                    return NotFound("Address not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content is the standard success response for a PUT
        }

        // DELETE: api/CustomerAddresses/5
        // Deletes an existing address
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAddress(string id)
        {
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound("Address not found.");
            }

            _context.CustomerAddresses.Remove(customerAddress);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content is standard for a successful deletion
        }

        // Helper method to check if an address exists before updating
        private bool CustomerAddressExists(string id)
        {
            return _context.CustomerAddresses.Any(e => e.CustomerAddressID == id);
        }
    }
}