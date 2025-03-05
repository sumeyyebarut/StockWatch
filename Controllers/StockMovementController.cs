using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
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
            var appDbContext = _context.StockMovements.Include(s => s.Product).Include(s => s.FromWarehouse).Include(s=>s.ToWarehouse);
            return View(await appDbContext.ToListAsync());
        }

       

        // GET: StockMovement/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["ToWarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name");
            ViewData["FromWarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name");

            return View();
        }

        // POST: StockMovement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ToWarehouseId,FromWareHouseId,Quantity,MovementType")] StockMovement stockMovement)
        {

            if(CheckStockForTransfer(stockMovement.ProductId,stockMovement.FromWarehouseId)){
                 _context.Add(stockMovement);
                await _context.SaveChangesAsync();
                //rediste yayınla
                if(CheckCriticalStock(stockMovement.ProductId,stockMovement.FromWarehouseId)){
                    //mail gönder  
                }
                return RedirectToAction(nameof(Index));
            }else{
                return BadRequest(new { message = "Lütfen geçerli bir ürün adedi giriniz." });

            }
               
        }
        private bool CheckStockForTransfer(int productId,int warehouseId)
        {
            if (GetProductStock(productId,warehouseId)!=null)
            {
                return true;
            }
            return false;
        }

        private ProductStock GetProductStock(int productId,int warehouseId)
        {
            var result= _context.ProductStocks.Where(x => x.ProductId == productId && x.WarehouseId == warehouseId && x.IsActive == true).FirstOrDefault();
            return result;
        }

        private bool CheckCriticalStock(int productId,int warehouseId){
            var result=GetProductStock(productId,warehouseId);
            if(result.StockQuantity<=result.CriticalStock){
                return true;
            }
            return false;
        }

       

      

       
    }
}
