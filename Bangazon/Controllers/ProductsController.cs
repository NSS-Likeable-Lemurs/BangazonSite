using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);




        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.ProductType).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,DateCreated,Description,Title,Price,Quantity,UserId,City,ImagePath,ProductTypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,DateCreated,Description,Title,Price,Quantity,UserId,City,ImagePath,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }


        [Authorize]
        public async Task<IActionResult> AddToOrder([FromRoute] int id)
        {
            // Find the product requested
            Product productToAdd = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == id);

            // Get the current user
            var user = await GetCurrentUserAsync();

            // See if the user has an open order
            var openOrder = await _context.Order.SingleOrDefaultAsync(o => o.User == user && o.PaymentTypeId == null);


            // If no order, create one, else add to existing order
            Order currentOrder;

            if (openOrder == null)
            {
                currentOrder = new Order();
                currentOrder.UserId = user.Id;
                currentOrder.PaymentTypeId = null;
                _context.Add(currentOrder);
                await _context.SaveChangesAsync();
            }

            else
            {
                currentOrder = openOrder;
            }

            OrderProduct currentProduct = new OrderProduct();
            currentProduct.ProductId = id;
            currentProduct.OrderId = currentOrder.OrderId;
            _context.Add(currentProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products");

        }






        //currentOrder = new Order();
        //currentOrder.UserId = user.Id;
        //        currentOrder.PaymentTypeId = null;
        //        _context.Add(currentOrder);
        //        await _context.SaveChangesAsync();



        //OrderProduct currentProduct = new OrderProduct();
        //currentProduct.ProductId = id;
        //    currentProduct.OrderId = currentOrder.OrderId;
        //    _context.Add(currentProduct);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Products");







        //OrderProduct newOrderedProduct = new OrderProduct()
        //{
        //    OrderId = openOrder.OrderId,
        //    ProductId = id
        //};


        //OrderProduct currentProduct = new OrderProduct();
        //_context.Add(currentProduct);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Orders");















        //currentProduct.ProductId = id;
        //    currentProduct.Orderid = currentOrder.OrderId;






        //public async Task<IActionResult> AddToOrder([FromRoute] int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser currentUser = await GetCurrentUserAsync();

        //        Order activeOrder = await _context.Order.Where(o => (o.UserId == currentUser.Id) && (o.PaymentTypeId == null)).FirstOrDefaultAsync();

        //        if (activeOrder != null)
        //        {
        //            OrderProduct newOrderedProduct = new OrderProduct()
        //            {
        //                OrderId = activeOrder.OrderId,
        //                ProductId = product.ProductId
        //            };

        //            _context.Add(newOrderedProduct);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(product);
        //}
    }
}
