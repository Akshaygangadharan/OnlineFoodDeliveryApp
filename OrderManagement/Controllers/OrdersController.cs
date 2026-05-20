
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderEntity = Orders.Models.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrdersContext context) : ControllerBase
{

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderEntity>>> Get(CancellationToken cancellationToken)
    {
        var list = await context.Orders.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(list);
    }

    // GET: api/orders/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderEntity>> Get(Guid id, CancellationToken cancellationToken)
    {
        var order = await context.Orders.AsNoTracking().FirstOrDefaultAsync(m => m.OrderId == id, cancellationToken);
        if (order == null)
            return NotFound();
        return order;
    }

    // GET: api/orders/user/{userId}
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<OrderEntity>>> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.AsNoTracking().Where(o => o.CustomerId == userId).ToListAsync(cancellationToken);
        return Ok(orders);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<OrderEntity>> Create(OrderEntity orders, CancellationToken cancellationToken)
    {
        if (orders == null)
            return BadRequest();

        if (orders.OrderId == Guid.Empty)
            orders.OrderId = Guid.NewGuid();

        context.Orders.Add(orders);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = orders.OrderId }, orders);
    }

    // PUT: api/orders/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, OrderEntity orders, CancellationToken cancellationToken)
    {
        if (id != orders.OrderId)
            return BadRequest();

        context.Entry(orders).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrdersExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // PUT: api/orders/{id}/status
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status, CancellationToken cancellationToken)
    {
        var order = await context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order == null)
            return NotFound();

        order.Status = status;
        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    // DELETE: api/orders/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var order = await context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order == null)
            return NotFound();

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private bool OrdersExists(Guid id)
    {
        return context.Orders.Any(e => e.OrderId == id);
    }
}
