using FastKart.DAL;
using FastKart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace FastKart.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public HeaderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var basketInString = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basketInString))
            {
                return View(new HeaderViewModel());
            }
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketInString);
            var newBasketViewModel = new List<BasketViewModel>();
            foreach (var item in basketViewModels)
            {
                var existProduct = _dbContext.Products.Find(item.ProductId);
                if (existProduct == null) continue;

                newBasketViewModel.Add(new BasketViewModel
                {
                    Name = existProduct.Name,
                    ProductId = existProduct.Id,
                    ImageUrl = existProduct.ImageUrl,
                    Price = existProduct.Price,
                    Count = item.Count

                });
            }
            var headerViewModel = new HeaderViewModel
            {
                Basket = newBasketViewModel,
                Count = newBasketViewModel.Sum(x=>x.Count),
                Sum = newBasketViewModel.Sum(x=>x.Count * x.Price)
            };
            
            return View(headerViewModel);
        }
    }
}
