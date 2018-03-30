using DPLRef.eCommerce.Contracts.WebStore.Catalog;
using DPLRef.eCommerce.Contracts.WebStore.Sales;

namespace DPLref.eCommerce.Tests.IntegrationTests.ExpectedResponses
{
    public static class WebstoreResponses
    {
        #region Catalog Responses
        // standard catalog response based upon the seed data 
        public static WebStoreCatalogResponse CatalogResponse { get; private set; }
            = new WebStoreCatalogResponse()
            {
                Success = true,
                Message = null,
                Catalog = new WebStoreCatalog()
                {
                    Id = 1,
                    Description = "TEST_CATALOG description",
                    Name = "TEST_CATALOG",
                    SellerName = "TEST_SELLER",
                    Products = new ProductSummary[]
                    {
                        new ProductSummary()
                        {
                            Id = 1,
                            Name = "TEST_PRODUCT",
                            Price = 5.99m
                        },
                        new ProductSummary()
                        {
                            Id = 2,
                            Name = "TEST_PRODUCT",
                            Price = 5.99m
                        }
                    }
                }
            };

        // "Catalog not found"
        public static WebStoreCatalogResponse CatalogNotFoundResponse { get; private set; }
            = new WebStoreCatalogResponse()
            {
                Success = false,
                Message = "Catalog not found",
                Catalog = null
            };

        // standard product response based upon the seed data
        public static WebStoreProductResponse ProductResponse { get; private set; }
            = new WebStoreProductResponse()
            {
                Success = true,
                Message = null,
                Product = new ProductDetail()
                {
                    Id = 1003,
                    Detail = "Used car from Santi's Used Car Emporium",
                    IsDownloadable = false,
                    Name = "2006 Chevrolet Suburban",
                    Price = 1011.00m,
                    ShippingWeight = 1.25m,
                    Summary = "Used 2006 Chevrolet Suburban",
                    SupplierName = "Chevrolet"
                }
            };

        // "Product not found"
        public static WebStoreProductResponse ProductNotFoundResponse { get; private set; }
            = new WebStoreProductResponse()
            {
                Success = false,
                Message = "Product not found",
                Product = null
            };
        #endregion
    }
}
