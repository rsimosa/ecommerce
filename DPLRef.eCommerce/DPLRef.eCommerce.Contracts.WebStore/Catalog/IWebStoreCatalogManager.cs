using DPLRef.eCommerce.Common.Shared;

// ReSharper disable once CheckNamespace
namespace DPLRef.eCommerce.Contracts.WebStore.Catalog
{

    public interface IWebStoreCatalogManager : IServiceContractBase
    {
        /// <summary>
        /// Return the webstore catalog detail and product summary information that is required to show a webstore page
        /// </summary>
        /// <returns></returns>
        WebStoreCatalogResponse ShowCatalog(int catalogId);

        /// <summary>
        /// Returns the details of a specific product
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        WebStoreProductResponse ShowProduct(int catalogId, int productId);
    }
}
