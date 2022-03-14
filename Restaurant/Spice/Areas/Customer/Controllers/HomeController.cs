using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spice.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        //161 for setting the session
        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.isActive == true).ToListAsync(),
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)// means the user has logged in before
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count();
                //assign it to the session
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }

            return View(IndexVM);
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


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == id);

            ShoppingCart cartObj = new ShoppingCart() // because we want to add an add to cart button in the Details view.
            {
                MenuItem = menuItemFromDb,
                MenuItemId = menuItemFromDb.Id,

            };
            return View(cartObj);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Details(ShoppingCart CartObject)
         {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;
                // to see if the item exist in the database
                ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId 
                && c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();
                if (cartFromDb==null) {
                    //does not exist so add a new entry
                    await _db.ShoppingCart.AddAsync(CartObject);
                }
                else
                {
                    //does exist so just increase the count
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;
                }
                await _db.SaveChangesAsync();

                // to store the total number of counts the user has in the session
                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();
                //to assign to a session
                HttpContext.Session.SetInt32("ssCartCount", count);
                return RedirectToAction("Index");
            }
            else
            {
                //basically returning the menuItem details view in case there is any error
                var menuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == CartObject.MenuItemId);

                ShoppingCart cartObj = new ShoppingCart() 
                {
                    MenuItem = menuItemFromDb,
                    MenuItemId = menuItemFromDb.Id,

                };
                return View(cartObj);

            }
        }
            
    }
}
