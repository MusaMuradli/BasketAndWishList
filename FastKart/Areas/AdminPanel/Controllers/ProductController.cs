using FastKart.DAL;
using FastKart.DAL.Entities;
using FastKart.Extencions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastKart.Areas.AdminPanel.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.Include(x=>x.Category).ToListAsync();
            return View(products);
        }

        public async  Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var categoryItems = new List<SelectListItem>();
            categories.ForEach(x => categoryItems.Add(new SelectListItem(x.Name, x.Id.ToString())));

            return View(new Product { CategoryListItems=categoryItems});
        }

         

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            var categories = await _dbContext.Categories.ToListAsync();
            var categoryItems = new List<SelectListItem>();
            categories.ForEach(x => categoryItems.Add(new SelectListItem(x.Name, x.Id.ToString())));

            product.CategoryListItems= categoryItems;

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if(product.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Please add image ");
                return View(product);
            }
            if (!product.ImageFile?.IsImage() ?? true)
            {
                ModelState.AddModelError("ImageFile", "Please add image format");

                return View(product);
            }

            if (!product.ImageFile?.CheckSize(2) ?? true)
            {
                ModelState.AddModelError("ImageFile", "Images length should be less than 1gb");

                return View(product);
            }
            var imageName = $"{Guid.NewGuid()} - {product.ImageFile?.FileName}";
            var rootPath = _webHostEnvironment.WebRootPath;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "fashion", "product", imageName);
            var fs = new FileStream(path, FileMode.Create);
            await product.ImageFile.CopyToAsync(fs);
            fs.Close();
            await _dbContext.Products.AddAsync(product);
            product.ImageUrl = imageName;

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            return View();
        }
    }
}
