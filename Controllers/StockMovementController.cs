using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockWatch.Data;
using StockWatch.Models;

namespace StockWatch.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly AppDbContext _context;

        public StockMovementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StockMovement
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.StockMovements.Include(s => s.CreatedByUser).Include(s => s.Product).Include(s => s.UpdatedByUser).Include(s => s.Warehouse);
            return View(await appDbContext.ToListAsync());
        }

        // GET: StockMovement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements
                .Include(s => s.CreatedByUser)
                .Include(s => s.Product)
                .Include(s => s.UpdatedByUser)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockMovement == null)
            {
                return NotFound();
            }

            return View(stockMovement);
        }

        // GET: StockMovement/Create
        public IActionResult Create()
        {
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id");
            return View();
        }

        // POST: StockMovement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,WarehouseId,Quantity,MovementType,IsActive,CreatedByUserId,UpdatedByUserId,CreatedAt,UpdatedAt")] StockMovement stockMovement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockMovement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.CreatedByUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stockMovement.ProductId);
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.UpdatedByUserId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", stockMovement.WarehouseId);
            return View(stockMovement);
        }

        // GET: StockMovement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement == null)
            {
                return NotFound();
            }
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.CreatedByUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stockMovement.ProductId);
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.UpdatedByUserId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", stockMovement.WarehouseId);
            return View(stockMovement);
        }

        // POST: StockMovement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,WarehouseId,Quantity,MovementType,IsActive,CreatedByUserId,UpdatedByUserId,CreatedAt,UpdatedAt")] StockMovement stockMovement)
        {
            if (id != stockMovement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockMovement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockMovementExists(stockMovement.Id))
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
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.CreatedByUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stockMovement.ProductId);
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id", stockMovement.UpdatedByUserId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", stockMovement.WarehouseId);
            return View(stockMovement);
        }

        // GET: StockMovement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements
                .Include(s => s.CreatedByUser)
                .Include(s => s.Product)
                .Include(s => s.UpdatedByUser)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockMovement == null)
            {
                return NotFound();
            }

            return View(stockMovement);
        }

        // POST: StockMovement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement != null)
            {
                _context.StockMovements.Remove(stockMovement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockMovementExists(int id)
        {
            return _context.StockMovements.Any(e => e.Id == id);
        }
    }
}
