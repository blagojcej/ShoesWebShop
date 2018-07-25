namespace ShoesWebShop.Web.WebMvc.Infrastructure
{
    public class ApiPaths
    {
        public static string GetAllCatalogItems(string baseUri, int pageIndex, int pageSize, int? brandId, int? typeId)
        {
            var filterQs = string.Empty;

            if (brandId.HasValue || typeId.HasValue)
            {
                var brandQs = (brandId.HasValue) ? brandId.Value.ToString() : "null";
                var typeQs = (typeId.HasValue) ? typeId.Value.ToString() : "null";
                filterQs = $"/type/{typeQs}/brand/{brandQs}";
            }

            return $"{baseUri}items{filterQs}?pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static string GetCatalogItem(string baseUri, int id)
        {
            return $"{baseUri}/items/{id}";
        }

        public static string GetAllBrands(string baseUri)
        {
            return $"{baseUri}/catalogBrands";
        }

        public static string GetAllTypes(string baseUri)
        {
            return $"{baseUri}/catalogTypes";
        }
    }
}
