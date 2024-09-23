using FastKart.DAL;
using FastKart.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FastKart.Areas.AdminPanel.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _contex;
        public CategoryController(AppDbContext contex)
        {
            _contex = contex;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _contex.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var category = await _contex.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isExist = await _contex.Categories.AnyAsync(x => x.Name.ToLower().Equals(category.Name.ToLower()));
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda kateqoriya movcuddur");
                return View();
            }
            await _contex.Categories.AddAsync(category);
            await _contex.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _contex.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _contex.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _contex.Categories.Remove(category);
            await _contex.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
