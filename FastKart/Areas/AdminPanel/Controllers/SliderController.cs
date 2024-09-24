using FastKart.DAL;
using FastKart.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastKart.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SliderController : Controller
{

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {

        var sliders = await _context.Sliders.ToListAsync();
        return View(sliders);
    }
    public IActionResult Create()
    {

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Slider slider)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
    

        if (!slider.ImageFile.ContentType.Contains("image"))
        {
            ModelState.AddModelError("ImageFile", "Please add image format");

            return View();
        }

        if ( slider.ImageFile.Length > 1024 * 1024)
        {
            ModelState.AddModelError("ImageFile", "Images length should be less than 1gb");

            return View();
        }
        var imageName = $"{Guid.NewGuid()} - {slider.ImageFile.FileName}";
        var rootPath = _webHostEnvironment.WebRootPath;
        var path = Path.Combine(rootPath, "assets", "images", "fashion", "banner", imageName);
       
        var fs= new FileStream(path, FileMode.Create);
        await slider.ImageFile.CopyToAsync(fs);
        fs.Close();
        slider.ImageUrl = imageName;
       
        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}
