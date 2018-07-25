using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoesWebShop.Web.WebMvc.Models;

namespace ShoesWebShop.Web.WebMvc.Services
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItems(int pageIndex, int pageSize, int? brandId, int? typeId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}