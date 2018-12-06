﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models.PaymentTypeViewModel;


namespace Bangazon.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.PaymentType).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Checkout
        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currentUser = await GetCurrentUserAsync();
            List<PaymentType> paymentTypes = _context.PaymentType.Where(pt=>pt.UserId == currentUser.Id).ToList();
            PaymentTypeViewModel model = new PaymentTypeViewModel(paymentTypes);

            return View(model);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }



        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderproduct = await _context.OrderProduct
                .Include(op => op.Order)
                .Include(op => op.Product)
                .FirstOrDefaultAsync(m => m.OrderProductId == id);

            return View(orderproduct);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCartConfirmed(OrderProduct op)
        {
            var orderproduct = await _context.OrderProduct.FindAsync(op.OrderProductId);
            _context.OrderProduct.Remove(orderproduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new {id=orderproduct.OrderId});
        }

        //update the payment type of order
        
        public async Task<IActionResult> CompleteOrder (PaymentTypeViewModel model)
        {
            if (model.PaymentType.PaymentTypeId == 0)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();



            Order currentOrder = _context.Order
               .Include(o => o.User)
                .Include(o => o.PaymentType)
                .Where(o => o.UserId == user.Id)
                .Where(o => o.PaymentType == null).ToList().FirstOrDefault();

            if(currentOrder != null)
            {
                currentOrder.PaymentTypeId = model.PaymentType.PaymentTypeId;
                _context.Update(currentOrder);
                await _context.SaveChangesAsync();
            }
            return View();
        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> CompleteOrder(int id, [Bind("OrderId,PaymentTypeId")] Order order)
        //        {
        //            if (id != order.OrderId)
        //            {
        //                return NotFound();
        //            }
        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _context.Update(order);
        //                    await _context.SaveChangesAsync();
        //    }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!OrderExists(order.OrderId))
        //                    {
        //                        return NotFound();
        //}
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction(nameof(Index));
        //            }
        //    ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "Description", order.PaymentTypeId);
        //    return View(order);
        //}
    }
}
