using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.ManagerUser)]
    public class CategoryController : Controller
    {       
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Get for Index
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.ToListAsync());
        }

        //Get for create
        public IActionResult create()
        {
            return View();
        }
        //Post for create
        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Create(Category category){
            if(ModelState.IsValid)
            {
                _db.Category.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
        //Get for edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }
        //Post for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category categroy)
        {
            if (ModelState.IsValid)
            {
               _db.Update(categroy);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(categroy);
            }

        }
        //Get for Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
            
        }
        //Post for Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete (Category category)
        {
            if (category==null) {
                return NotFound();
            }
            else
            {
                _db.Remove(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

    }
}
    

