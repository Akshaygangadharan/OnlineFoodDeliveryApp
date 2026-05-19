
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Models;

public class OrdersController : Controller
{
    private readonly OrdersContext _context;

    public OrdersController(OrdersContext context)
    {
        _context = context;
    }

    // GET: ORDERSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Orders.ToListAsync());
    }

    // GET: ORDERSS/Details/5
    public async Task<IActionResult> Details(System.Guid? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orders = await _context.Orders
            .FirstOrDefaultAsync(m => m.OrderId == orderid);
        if (orders == null)
        {
            return NotFound();
        }

        return View(orders);
    }

    // GET: ORDERSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ORDERSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrderId,CustomerId,RestaurantId,DeliveryManId,OrderDate,Status,TotalAmount,PaymentAddressId,DeliveryAddressId,ExpectedDeliveryTime,ActualDeliveryTime,CreatedAt,UpdatedAt")] Orders orders)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orders);
    }

    // GET: ORDERSS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orders = await _context.Orders.FindAsync(orderid);
        if (orders == null)
        {
            return NotFound();
        }
        return View(orders);
    }

    // POST: ORDERSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? orderid, [Bind("OrderId,CustomerId,RestaurantId,DeliveryManId,OrderDate,Status,TotalAmount,PaymentAddressId,DeliveryAddressId,ExpectedDeliveryTime,ActualDeliveryTime,CreatedAt,UpdatedAt")] Orders orders)
    {
        if (orderid != orders.OrderId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orders);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(orders.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(orders);
    }

    // GET: ORDERSS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var orders = await _context.Orders
            .FirstOrDefaultAsync(m => m.OrderId == orderid);
        if (orders == null)
        {
            return NotFound();
        }

        return View(orders);
    }

    // POST: ORDERSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? orderid)
    {
        var orders = await _context.Orders.FindAsync(orderid);
        if (orders != null)
        {
            _context.Orders.Remove(orders);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrdersExists(System.Guid? orderid)
    {
        return _context.Orders.Any(e => e.OrderId == orderid);
    }
}
