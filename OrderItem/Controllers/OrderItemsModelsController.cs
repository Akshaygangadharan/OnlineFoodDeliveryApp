
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderItems.Models;

public class OrderItemsModelsController : Controller
{
    private readonly OrderItemsContext _context;

    public OrderItemsModelsController(OrderItemsContext context)
    {
        _context = context;
    }

    // GET: ORDERITEMSMODELS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.OrderItems.ToListAsync());
    }

    // GET: ORDERITEMSMODELS/Details/5
    public async Task<IActionResult> Details(System.Guid? orderitemid)
    {
        if (orderitemid == null)
        {
            return NotFound();
        }

        var orderitemsmodel = await _context.OrderItems
            .FirstOrDefaultAsync(m => m.OrderItemId == orderitemid);
        if (orderitemsmodel == null)
        {
            return NotFound();
        }

        return View(orderitemsmodel);
    }

    // GET: ORDERITEMSMODELS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ORDERITEMSMODELS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrderItemId,OrderId,ProductId,RestuarantId,Quantity,UnitPrice,Discount,TaxAmount,ItemDescription,SubTotal,SpecialInstructions,Status,CreatedAt,LastUpdatedAt")] OrderItemsModel orderitemsmodel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orderitemsmodel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orderitemsmodel);
    }

    // GET: ORDERITEMSMODELS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? orderitemid)
    {
        if (orderitemid == null)
        {
            return NotFound();
        }

        var orderitemsmodel = await _context.OrderItems.FindAsync(orderitemid);
        if (orderitemsmodel == null)
        {
            return NotFound();
        }
        return View(orderitemsmodel);
    }

    // POST: ORDERITEMSMODELS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? orderitemid, [Bind("OrderItemId,OrderId,ProductId,RestuarantId,Quantity,UnitPrice,Discount,TaxAmount,ItemDescription,SubTotal,SpecialInstructions,Status,CreatedAt,LastUpdatedAt")] OrderItemsModel orderitemsmodel)
    {
        if (orderitemid != orderitemsmodel.OrderItemId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orderitemsmodel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemsModelExists(orderitemsmodel.OrderItemId))
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
        return View(orderitemsmodel);
    }

    // GET: ORDERITEMSMODELS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? orderitemid)
    {
        if (orderitemid == null)
        {
            return NotFound();
        }

        var orderitemsmodel = await _context.OrderItems
            .FirstOrDefaultAsync(m => m.OrderItemId == orderitemid);
        if (orderitemsmodel == null)
        {
            return NotFound();
        }

        return View(orderitemsmodel);
    }

    // POST: ORDERITEMSMODELS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? orderitemid)
    {
        var orderitemsmodel = await _context.OrderItems.FindAsync(orderitemid);
        if (orderitemsmodel != null)
        {
            _context.OrderItems.Remove(orderitemsmodel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderItemsModelExists(System.Guid? orderitemid)
    {
        return _context.OrderItems.Any(e => e.OrderItemId == orderitemid);
    }
}
