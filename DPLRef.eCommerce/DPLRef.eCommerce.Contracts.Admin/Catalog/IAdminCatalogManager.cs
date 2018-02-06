using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Catalog
{
    public interface IAdminCatalogManager : IServiceContractBase
    {
        /// <summary>
        /// Shows the list of catalogs for the authenticated seller
        /// </summary>
        /// <returns></returns>
        AdminCatalogsResponse FindCatalogs();

        /// <summary>
        /// Shows a specific catalog for the authenticated seller
        /// </summary>
        /// <returns></returns>
        AdminCatalogResponse ShowCatalog(int catalogId);

        /// <summary>
        /// Updates a specific catalog for the authenticated seller
        /// </summary>
        AdminCatalogResponse SaveCatalog(WebStoreCatalog catalog);

        /// <summary>
        /// Shows a specific product for the authenticated seller
        /// </summary>
        /// <returns></returns>
        AdminProductResponse ShowProduct(int catalogId, int productId);

        /// <summary>
        /// Updates a specific product for the authenticated seller
        /// </summary>
        AdminProductResponse SaveProduct(int catalogId, Product product);
    }


}
