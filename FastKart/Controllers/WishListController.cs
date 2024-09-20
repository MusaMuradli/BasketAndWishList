using FastKart.DAL;
using FastKart.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastKart.Controllers
{
    public class WishListController : Controller
    {
        private readonly AppDbContext _dbContext;

        public WishListController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var wishlistInString = Request.Cookies["wishlist"];
            if (string.IsNullOrEmpty(wishlistInString))
            {
                return Json(new List<WishListViewModel>());
            }

            var wishlistViewModels = JsonConvert.DeserializeObject<List<WishListViewModel>>(wishlistInString);
            var newWishlistViewModels = new List<WishListViewModel>();

            foreach (var item in wishlistViewModels)
            {
                var existProduct = _dbContext.Products.Find(item.ProductId);
                if (existProduct == null) continue;

                newWishlistViewModels.Add(new WishListViewModel
                {
                    Name = existProduct.Name,
                    ProductId = existProduct.Id,
                    ImageUrl = existProduct.ImageUrl,
                    Price = existProduct.Price,
                    Count = item.Count
                });
            }

            return View(newWishlistViewModels);
        }
        public async Task<IActionResult> AddToWishlist(int? id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null) return BadRequest();

            var wishlistViewModels = new List<WishListViewModel>();

            if (string.IsNullOrEmpty(Request.Cookies["wishlist"]))
            {
                wishlistViewModels.Add(new WishListViewModel
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
                wishlistViewModels = JsonConvert.DeserializeObject<List<WishListViewModel>>(Request.Cookies["wishlist"]);

                var existProduct = wishlistViewModels.Find(x => x.ProductId == product.Id);
                if (existProduct == null)
                {
                    wishlistViewModels.Add(new WishListViewModel
                    {
                        Name = product.Name,
                        ProductId = product.Id,
                        ImageUrl = product.ImageUrl, 
                        Price = product.Price,
                        Count = 1
                    });
                }
            }

            Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(wishlistViewModels));

            return RedirectToAction("Index", "Home");
        }
    }
}
