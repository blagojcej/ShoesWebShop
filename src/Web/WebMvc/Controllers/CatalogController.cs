using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShoesWebShop.Web.WebMvc.Services;
using ShoesWebShop.Web.WebMvc.ViewModels;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSvc)
        {
            _catalogSvc = catalogSvc;
        }

        public async Task<IActionResult> Index(int? BrandFilterApplied, int? TypeFilterApplied, int? pageIndex)
        {
            int itemsPerPage = 10;
            var catalog =
                await _catalogSvc.GetCatalogItems(pageIndex ?? 1, itemsPerPage, BrandFilterApplied, TypeFilterApplied);
            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogSvc.GetBrands(),
                Types = await _catalogSvc.GetTypes(),
                BrandFilterApplied = BrandFilterApplied ?? 0,
                TypeFilterApplied = TypeFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = pageIndex ?? 1,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPerPage)
                }
            };

            vm.PaginationInfo.Next =
                (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";


            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application descripton page.";

            return View();
        }
    }
}
