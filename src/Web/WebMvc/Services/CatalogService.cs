using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoesWebShop.Web.WebMvc.Infrastructure;
using ShoesWebShop.Web.WebMvc.Models;

namespace ShoesWebShop.Web.WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _httpClient;
        private readonly ILogger<CatalogService> _logger;
        private string _remoteServiceBaseUrl;

        public CatalogService(IOptionsSnapshot<AppSettings> settings, IHttpClient httpClient, ILogger<CatalogService> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/catalog";
        }

        public async Task<Catalog> GetCatalogItems(int pageIndex, int pageSize, int? brandId, int? typeId)
        {
            var allCatalogItemsUri =
                ApiPaths.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, pageIndex, pageSize, brandId, typeId);

            var dataString = await _httpClient.GetStringAsync(allCatalogItemsUri);

            var response = JsonConvert.DeserializeObject<Catalog>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            var getBrandsUri = ApiPaths.Catalog.GetAllBrands(_remoteServiceBaseUrl);

            var dataString = await _httpClient.GetStringAsync(getBrandsUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem() {Value = null, Text = "All", Selected = true}
            };

            var brands = JArray.Parse(dataString);

            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("brand")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var getTypesUri = ApiPaths.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var dataString = await _httpClient.GetStringAsync(getTypesUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem() {Value = null, Text = "All", Selected = true}
            };

            var types = JArray.Parse(dataString);

            foreach (var type in types.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = type.Value<string>("id"),
                    Text = type.Value<string>("type")
                });
            }

            return items;
        }
    }
}
