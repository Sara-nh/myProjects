﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ManagerUser)]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }
                 
        public async Task<IActionResult> Index()
        {
            return View(await _db.Coupon.ToListAsync());
        }
        //Get for Create
        public IActionResult Create()
        {
            return View();
        }
        //post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(Coupon coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using(var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1=new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                        coupons.Picture = p1;
                    }                   
                }
                _db.Coupon.Add(coupons);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(coupons);
        }
        //Get for Edit
        public async Task<IActionResult>Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await _db.Coupon.SingleOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        //Post for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupon)
        {
            if(coupon.Id==0)
            {
                return NotFound();
            }
            var couponFromDb = await _db.Coupon.Where(c => c.Id == coupon.Id).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
               var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using( var fs1 = files[0].OpenReadStream())
                    {
                        using(var ms1=new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    couponFromDb.Picture = p1;
                }
                couponFromDb.MinimumAmount = coupon.MinimumAmount;
                couponFromDb.Name = coupon.Name;
                couponFromDb.Discount = coupon.Discount;
                couponFromDb.CouponType = coupon.CouponType;
                couponFromDb.isActive = coupon.isActive;


                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(coupon);
        }
        //Get for Detail
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await _db.Coupon.FirstOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
           
        }
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await _db.Coupon.SingleOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        //Post for Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete(Coupon coupon)
        {
            if (coupon.Id == 0)
            {
                return NotFound();
            }
            var couponFromDb = await _db.Coupon.SingleOrDefaultAsync(c => c.Id == coupon.Id);
            if (couponFromDb == null)
            {
                return NotFound();
            }
            _db.Coupon.Remove(couponFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
