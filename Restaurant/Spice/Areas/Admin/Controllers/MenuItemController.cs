using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;
using Spice.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{//112

    [Area("Admin")]
    [Authorize(Roles = SD.ManagerUser)]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            //initialization 
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category=_db.Category,
                MenuItem=new Models.MenuItem()
            };            
        }
        public async Task<IActionResult>Index()
        {
            var menuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).ToListAsync();
            return View(menuItem);
        }
        //Get for Create
        public IActionResult create()//here instead of passing the MenuItem view model as an argument, we're using bind property directly.
        {
            return View(MenuItemVM);

        }
        //Post for Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
                                                        //when push the form MenuItemVM will be loaded by defalut
        public async Task<IActionResult> CreatePost() //can't call it create because we have anouther method with the exact same parameters.
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }
            _db.MenuItem.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            //work on image saving section

            string webRootPath = _hostingEnvironment.WebRootPath; //getting the path
            var files = HttpContext.Request.Form.Files; // extracting all the downloaded images
            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id); //extracting menuItem from the database
            if (files.Count > 0)
            {
                // image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;

            }
            else
            {
                //no image has been uploaded
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + MenuItemVM.MenuItem.Id+".png");//even though the destination is the same in case we need to change it
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";                    
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //Get for Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            {
                return NotFound();    
            }
            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            MenuItemVM.SubCategory=await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
            
            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }

        //Post for Edit(116)
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> EditPost(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemVM);// assign the SubCategory before returning
            }
            
            //work on image saving section

            string webRootPath = _hostingEnvironment.WebRootPath; //getting the path
            var files = HttpContext.Request.Form.Files; // getting the files that were uploaded from the server
            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id); //extracting menuItem from the database
            if (files.Count > 0)
            {
                // new image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                if (menuItemFromDb.Image != null)
                {
                    // Delete the original image
                    var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }


                // Upload the new image
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension_new;

            }
            // no image has been uploaded, apply the changes user's made to text or dorpdowns
            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //GET : Details MenuItem
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        //GET : Delete MenuItem
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        //POST Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            MenuItem menuItem = await _db.MenuItem.FindAsync(id);

            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath, menuItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.MenuItem.Remove(menuItem);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
