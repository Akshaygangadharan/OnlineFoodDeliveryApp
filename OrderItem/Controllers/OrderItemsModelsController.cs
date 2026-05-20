
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsModelsController(OrderItemsContext context) : ControllerBase
{

    // GET: api/orderitemsmodels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItemsModel>>> Get(CancellationToken cancellationToken)
    {
        var list = await context.OrderItems.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(list);
    }

    // GET: api/orderitemsmodels/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderItemsModel>> Get(Guid id, CancellationToken cancellationToken)
    {
        var item = await context.OrderItems.AsNoTracking().FirstOrDefaultAsync(m => m.OrderItemId == id, cancellationToken);
        if (item == null)
            return NotFound();
        return item;
    }

    // GET: api/orderitemsmodels/order/{orderId}
    [HttpGet("order/{orderId:guid}")]
    public async Task<ActionResult<IEnumerable<OrderItemsModel>>> GetByOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var items = await context.OrderItems.AsNoTracking().Where(i => i.OrderId == orderId).ToListAsync(cancellationToken);
        return Ok(items);
    }

    // POST: api/orderitemsmodels
    [HttpPost]
    public async Task<ActionResult<OrderItemsModel>> Create(OrderItemsModel orderitemsmodel, CancellationToken cancellationToken)
    {
        if (orderitemsmodel == null)
            return BadRequest();

        if (orderitemsmodel.OrderItemId == Guid.Empty)
            orderitemsmodel.OrderItemId = Guid.NewGuid();

        context.OrderItems.Add(orderitemsmodel);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = orderitemsmodel.OrderItemId }, orderitemsmodel);
    }

    // PUT: api/orderitemsmodels/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, OrderItemsModel orderitemsmodel, CancellationToken cancellationToken)
    {
        if (id != orderitemsmodel.OrderItemId)
            return BadRequest();

        context.Entry(orderitemsmodel).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderItemsModelExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/orderitemsmodels/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var item = await context.OrderItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return NotFound();

        context.OrderItems.Remove(item);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private bool OrderItemsModelExists(Guid id)
    {
        return context.OrderItems.Any(e => e.OrderItemId == id);
    }
}
