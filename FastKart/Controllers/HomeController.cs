using FastKart.DAL;
using FastKart.DAL.Entities;
using FastKart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FastKart.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories= _dbContext.Categories.ToList();
            var products = _dbContext.Products.ToList();
            HttpContext.Session.SetString("session", "Hello");
            Response.Cookies.Append("cookie", "cookieValue", new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(5)});
            var model = new HomeViewModel()
            {
                Categories = categories,
                Products = products
            };
            return View(model);
        }

        public IActionResult Basket() 
        {
            //var sessionValue = HttpContext.Session.GetString("session");
            //var cookieValue = Request.Cookies["cookie"];

            var basketInString = Request.Cookies["basket"];
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketInString);
            var newBasketViewModels=new List<BasketViewModel>();
            foreach (var item in basketViewModels)
            {
                var existProduct = _dbContext.Products.Find(item.ProductId);
                if (existProduct == null) continue;
                newBasketViewModels.Add(new BasketViewModel
                {
                    Name = existProduct.Name,
                    ProductId = existProduct.Id,
                    ImageUrl = existProduct.ImageUrl,
                    Price = existProduct.Price,
                    Count = item.Count

                });
            }


            return Json(newBasketViewModels);
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null) return BadRequest(); 

            var basketViewModels= new List<BasketViewModel>();

            if (string.IsNullOrEmpty(Request.Cookies["basket"]))
            {
                basketViewModels.Add(new BasketViewModel
                {
                    Name = product.Name,
                    ProductId = product.Id,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Count = 1
                });
            }
            else
            {
                basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]); //evvelce yaranmis produtdin listi
                var existProduct = basketViewModels.Find(x=>x.ProductId == product.Id);
                if (existProduct == null)
                {
                    basketViewModels.Add(new BasketViewModel
                    {
                        Name = product.Name,
                        ProductId = product.Id,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        Count = 1
                    });
                }
                else
                {
                    existProduct.Count++;
                    existProduct.Price=product.Price;
                    existProduct.Name= product.Name;
                    existProduct.ImageUrl = product.ImageUrl;


                }
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketViewModels)); // bu kod setri ifde ve elsede yazilmalidi

            //return RedirectToAction(nameof(Index));
            return Json(new {basketViewModels, Count=basketViewModels.Sum(x=>x.Count), Sum=basketViewModels.Sum(y=>y.Count * y.Price)});
        }

        public async Task<IActionResult> CreateSlider()
        {

            return View();
        }

       

    }
}