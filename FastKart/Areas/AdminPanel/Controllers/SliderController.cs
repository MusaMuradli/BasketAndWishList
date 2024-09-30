using FastKart.Constans;
using FastKart.DAL;
using FastKart.DAL.Entities;
using FastKart.Extencions;
using Microsoft.AspNetCore.Authorization;
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
    

        if (!slider.ImageFile.IsImage())
        {
            ModelState.AddModelError("ImageFile", "Please add image format");

            return View();
        }

        if (!slider.ImageFile.CheckSize(2))
        {
            ModelState.AddModelError("ImageFile", "Images length should be less than 1gb");

            return View();
        }

        var imageName = await slider.ImageFile.GenerateFileAsync(FileConstans.SliderImagePath );
        slider.ImageUrl = imageName;
       
        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}
