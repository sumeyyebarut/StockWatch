using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using StockWatch.Data;
using StockWatch.Models;
using StockWatch.Services.Abstract;

namespace StockWatch.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailProvider _emailProvider;
        private readonly IMemoryCache _memoryCache;
        //private readonly ConnectionMultiplexer _redis;
        private readonly ISubscriber _publisher;

        public StockMovementController(AppDbContext context,IEmailProvider emailProvider,IMemoryCache memoryCache,
        IConnectionMultiplexer redis)
        {
            _context = context;
            _emailProvider=emailProvider;   
            _memoryCache = memoryCache;
            _publisher = redis.GetSubscriber();
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
        public async Task<IActionResult> Create([Bind("ProductId,ToWarehouseId,FromWarehouseId,Quantity,MovementType")] StockMovement stockMovement)
        { 
            
             var stockCheck=CheckStockForTransfer(stockMovement.ProductId,stockMovement.FromWarehouseId);
                if(stockCheck!=null){
                 _context.Add(stockMovement);
                stockCheck.StockQuantity=stockCheck.StockQuantity-stockMovement.Quantity;
                _context.ProductStocks.Update(stockCheck);
                await _context.SaveChangesAsync();

                //rediste yayınla
                var redismessage = JsonSerializer.Serialize(stockMovement);
                _publisher.Publish("stock-movement", redismessage);
                if(CheckCriticalStock(stockMovement.ProductId,stockMovement.FromWarehouseId)){

            var message = new Message("Kritik stok bildirimi",stockMovement.ProductId +"idli urunun stogu kritik seviyenin altina dusmustur.");
            _emailProvider.SendEmail(message);
                }
                return RedirectToAction(nameof(Index));
            }else{
                return BadRequest(new { message = "Transfer yapılmak istenen depodaki ürün adedi transfer için uygun değildir." });

            }
        
           
               
        }
        private ProductStock CheckStockForTransfer(int productId,int warehouseId)
        {var result =GetProductStock(productId,warehouseId);
            if (result!=null)
            {
                return result;
            }
            return null;
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
