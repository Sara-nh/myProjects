using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
//using Spice.Service;
using Spice.Utility;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Spice.Areas.Customer.Controllers
{   //167
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        //225
        private readonly IEmailSender _emailSender;
        public CartController(ApplicationDbContext db,IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }
        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }


        public async Task<IActionResult> Index()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailCart.OrderHeader.OrderTotal = 0;
            //to get the userId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //to get all the items from shoppingCart
            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            //to add that to the detailCart
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }
            // to display the description and calculate the OrderTotal
            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                // convert the description
                if (list.MenuItem.Description != null)
                {
                    list.MenuItem.Description = SD.ConvertToRawHtml(list.MenuItem.Description);
                    if (list.MenuItem.Description.Length > 100)
                    {
                        list.MenuItem.Description = list.MenuItem.Description.Substring(0, 99) + "...";
                    }
                }
            }
            //no coupon used yet, both totals should be equal 
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;
            //get the start session coupon

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync(); //toUpper or ToLower doesnt matter
                //call the static details function
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }
            return View(detailCart);
        }
        //171
        public IActionResult AddCoupon()
        {
            if (detailCart.OrderHeader.CouponCode == null)
            {
                detailCart.OrderHeader.CouponCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, detailCart.OrderHeader.CouponCode);

            return RedirectToAction("Index");
        }
        //172
        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.SetString(SD.ssCouponCode, string.Empty);

            return RedirectToAction("Index");
        }
        //173
        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId); //retrieve the cart object
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId); //retrieve the cart object
            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                await _db.SaveChangesAsync();
                var cnt = _db.ShoppingCart.Where(c => c.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);

            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId); //retrieve the cart object

            _db.ShoppingCart.Remove(cart);
            await _db.SaveChangesAsync();
            var cnt = _db.ShoppingCart.Where(c => c.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            return RedirectToAction("Index");
        }
        //174
        public async Task<IActionResult> Summary()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailCart.OrderHeader.OrderTotal = 0;
            //to get the userId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //to access information inside applicationUser such as name and phone
            ApplicationUser applicationUser = await _db.ApplicationUser.Where(u => u.Id == claim.Value).FirstOrDefaultAsync();

            //to get all the items from shoppingCart
            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);

            //to add that to the detailCart
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }
           
            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                
            }
            //no coupon used yet, both totals should be equal 
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            detailCart.OrderHeader.PickUpName = applicationUser.Name;
            detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            detailCart.OrderHeader.PickupTime = DateTime.Now;//will modify this using javascrip for time

            //get the start session coupon

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync(); //toUpper or ToLower doesnt matter
                //call the static details function
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }
            return View(detailCart);
        }
        //180 (copied from Index)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeToken)  
        {
            //to get the userId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //to get all the items from shoppingCart
            detailCart.listCart = await _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();
            
            detailCart.OrderHeader.PaymentStatus=SD.PaymentStatusPending;
            detailCart.OrderHeader.OderDate= DateTime.Now;
            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.Status =SD.PaymentStatusPending;
            detailCart.OrderHeader.PickupTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString()+" " + detailCart.OrderHeader.PickupTime.ToShortTimeString()) ; //merge date and time 

            //to create a list of orderDetails
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailCart.OrderHeader);
            await _db.SaveChangesAsync();

            detailCart.OrderHeader.OrderTotalOriginal = 0;

            foreach (var item in detailCart.listCart)
            {
                item.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails()
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.MenuItem.Description,
                    Name=item.MenuItem.Name,
                    Price=item.MenuItem.Price,
                    Count=item.Count,                
                };
                detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);
            }
            

            //get the start session coupon

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync(); //toUpper or ToLower doesnt matter
                //call the static details function
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal;
            }
            detailCart.OrderHeader.CouponCodeDiscount = detailCart.OrderHeader.OrderTotalOriginal - detailCart.OrderHeader.OrderTotalOriginal;
           
            _db.ShoppingCart.RemoveRange(detailCart.listCart); //empty the shopping card and save the changes
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, 0);
            await _db.SaveChangesAsync();
            //184
            var options = new ChargeCreateOptions()
            {
                Amount=Convert.ToInt32(detailCart.OrderHeader.OrderTotal*100),
                Currency="cad",
                Description="Order ID:"+detailCart.OrderHeader.Id,
                Source= stripeToken
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            if(charge.BalanceTransactionId==null) //means there is some error while charging the credit card
            {
                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }
            else
            {
                detailCart.OrderHeader.TransactionId = charge.BalanceTransactionId;//else just save the transaction Id
            }
            if (charge.Status.ToLower() == "succeeded")
            {

                //225-email for successful order
                await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email, "Spice-Order Created" + detailCart.OrderHeader.Id.ToString(), "Order has been submitted successfully.");

                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                detailCart.OrderHeader.Status = SD.StatusSubmitted;
            }
            else
            {
                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }
            await _db.SaveChangesAsync();

            //return RedirectToAction("Index", "Home");

            return RedirectToAction("Confirm", "Order", new { id = detailCart.OrderHeader.Id });
        }        
    }
}
