using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersManagementController : ControllerBase
    {
        private readonly OrderManagementContext _context;

        public OrdersManagementController(OrderManagementContext context)
        {
            _context = context;
        }

        // GET: api/OrdersManagementModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersManagementModel>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/OrdersManagementModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersManagementModel>> GetOrdersManagementModel(string id)
        {
            var ordersManagementModel = await _context.Orders.FindAsync(id);

            if (ordersManagementModel == null)
            {
                return NotFound();
            }

            return ordersManagementModel;
        }

        // PUT: api/OrdersManagementModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdersManagementModel(string id, OrdersManagementModel ordersManagementModel)
        {
            if (id != ordersManagementModel.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(ordersManagementModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersManagementModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrdersManagementModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrdersManagementModel>> PostOrdersManagementModel(OrdersManagementModel ordersManagementModel)
        {
            _context.Orders.Add(ordersManagementModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrdersManagementModelExists(ordersManagementModel.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrdersManagementModel", new { id = ordersManagementModel.OrderId }, ordersManagementModel);
        }

        // DELETE: api/OrdersManagementModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdersManagementModel(string id)
        {
            var ordersManagementModel = await _context.Orders.FindAsync(id);
            if (ordersManagementModel == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(ordersManagementModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersManagementModelExists(string id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
