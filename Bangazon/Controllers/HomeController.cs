using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Models;
using Bangazon.Data;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        /**
 * Class: HomeController
 * Purpose: Define all methods that interract with the User table in the database, and routes to create
            routes to create new users.
 * Author: Helen Chalmers
 * Methods:
 *   HomeController(BangazonDeltaContext) - Constructor that displays 20 newest made products that are linked to their respective product detail page.
 *   uses OrderByDescending and Take methods.
 *  
 */
        // GET: Cohorts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.OrderByDescending(x => x.DateCreated).Take(20).ToListAsync());
        }
        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
