using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spice.Areas.Customer
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        //226
        private readonly IEmailSender _emailSender;

        private int PageSize = 2; //displays only 2 items per page
        public OrderController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        //186
        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);//to get the id of the logged-in user
            OrderDetailsViewModel OrderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == id).ToListAsync(),
            };
            return View(OrderDetailsViewModel);

        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        //190, 197 for pagination
        [Authorize]
        public async Task<IActionResult> OrderHistory(int productPage=1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); //retreive the id of the logged-in user

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders= new List<OrderDetailsViewModel>()

            };
          
            List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(o => o.UserId == claim.Value).ToListAsync();
            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync(),
                };
                orderListVM.Orders.Add(individual);
            }
            //setting the paging info
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(p => p.OrderHeader.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();
            orderListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParam="/Customer/Order/OrderHistory?productPage=:"

            };

            return View(orderListVM);
        }
        public async Task<IActionResult>GetOrderDetails(int Id)
        {
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {              
                OrderHeader = await _db.OrderHeader.Include(el => el.ApplicationUser).FirstOrDefaultAsync(m => m.Id == Id),
                OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == Id).ToListAsync()
             };
            orderDetailsViewModel.OrderHeader.ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == orderDetailsViewModel.OrderHeader.UserId);
            return PartialView("_IndividualOrderDetails",orderDetailsViewModel);
        }

        //201
        [Authorize(Roles =SD.KitchenUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> ManageOrder()
        {
            List<OrderDetailsViewModel> orderDetailsVM = new List<OrderDetailsViewModel>();
              
            //get the list
            List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Where(o =>o.Status ==SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                                                                     .OrderByDescending(o=>o.PickupTime)
                                                                    .ToListAsync();
            //iterate through each item 
            foreach (OrderHeader item in OrderHeaderList)
            {
                //fetch OrderDetailsViewModel
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync(),
                };
                orderDetailsVM.Add(individual);
            }
            

            return View(orderDetailsVM.OrderBy(o=>o.OrderHeader.PickupTime).ToList());
        }




        //204
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]

        public async Task<IActionResult> OrderPrepare(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.Where(o => o.Id == OrderId).FirstOrDefaultAsync(); //or await _db.OrderHeade.FindAsync(OrderId)
            orderHeader.Status = SD.StatusInProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder");
        }

        //204
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]

        public async Task<IActionResult> OrderReady(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.Where(o => o.Id == OrderId).FirstOrDefaultAsync();
            orderHeader.Status = SD.StatusReady;
            await _db.SaveChangesAsync();

            //226
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice-Order Ready For Pickup" + orderHeader.Id.ToString(), "Order is ready for pickup.");

            return RedirectToAction("ManageOrder");
        }

        //204
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]

        public async Task<IActionResult> OrderCancel(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.Where(o => o.Id == OrderId).FirstOrDefaultAsync();
            orderHeader.Status = SD.StatusCancelled;
            await _db.SaveChangesAsync();
            //226
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice-Order Canceled" + orderHeader.Id.ToString(), "Order has been canceled.");

            return RedirectToAction("ManageOrder");
        }

        //Get for OrderPickup
        //206-208:adding search parameters
        [Authorize]
        public async Task<IActionResult> OrderPickup(int productPage = 1,string searchEmail=null, string searchPhone = null,string searchName= null)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); //retreive the id of the logged-in user

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()

            };

            StringBuilder param = new StringBuilder();
            param.Append("/Customer/Order/OrderPickup?productPage=:");
            param.Append("&searchName=");
            if(searchName!=null)
            {
                param.Append(searchName);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }

            List<OrderHeader> OrderHeaderList = new List<OrderHeader>();

            if (searchName != null || searchEmail != null || searchPhone != null) // then retrieve a new order list
            {
                var user = new ApplicationUser();
                
               
                //user can search by either Name,Email or Phone,not all of them together
                if (searchName != null)
                {
                    OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser)
                        .Where(u => u.PickUpName.ToLower().Contains(searchName.ToLower()))
                        .OrderByDescending(o => o.OderDate).ToListAsync();
                }
                else
                {
                    if (searchEmail != null)
                    {
                        user = await _db.ApplicationUser
                            .Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()))
                            .FirstOrDefaultAsync();

                        OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser)
                        .Where(u => u.UserId == user.Id)
                        .OrderByDescending(o => o.OderDate).ToListAsync();
                    }
                    else
                    {
                        if (searchPhone != null)
                        {
                            OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser)
                                .Where(u => u.PhoneNumber.Contains(searchPhone))
                                .OrderByDescending(o => o.OderDate).ToListAsync();
                        }
                    }

                }
            }
            else
            {
                 OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(o => o.Status == SD.StatusReady).ToListAsync();
            }
                foreach (OrderHeader item in OrderHeaderList)
                {
                    OrderDetailsViewModel individual = new OrderDetailsViewModel
                    {
                        OrderHeader = item,
                        OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync(),
                    };
                    orderListVM.Orders.Add(individual); //gives the list of all orders that are ready to pickup
                }
            
            //setting the paging info
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(p => p.OrderHeader.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();
            orderListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParam = param.ToString(),

            };

            return View(orderListVM);
        }

        //post for OrderPickup
        [Authorize(Roles = SD.ManagerUser + "," + SD.FrontDeskUser)]
        [HttpPost]
        [ActionName("OrderPickup")]
        public async Task<IActionResult> OrderPickupPost(int orderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            orderHeader.Status = SD.Statuscompleted;
            await _db.SaveChangesAsync();

            //226
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice-Order Completed" + orderHeader.Id.ToString(), "Order has been completed successfully.");

            return RedirectToAction("OrderPickup");
        }
        
    }
}
