using DPLRef.eCommerce.Contracts.Admin.Catalog;

namespace DPLref.eCommerce.Tests.IntegrationTests.ExpectedResponses
{
    public static class AdminResponses
    {
        #region Catalog Responses

        // standard list of catalogs response based upon the seed data
        public static AdminCatalogsResponse FindCatalogsResponse { get; private set; }
            = new AdminCatalogsResponse()
            {
                Success = true,
                    Message = null,
                    Catalogs = new WebStoreCatalog[]
                    {
                        new WebStoreCatalog()
                        {
                            Id = 1,
                            Name = "TEST_CATALOG",
                            Description = "TEST_CATALOG description"
                        },
                        new WebStoreCatalog()
                        {
                            Id = 2,
                            Name = "TEST_CATALOG_2",
                            Description = "TEST_CATALOG_2 description"
                        }
                    }
            };

        // "Seller not authenticated"
        public static AdminCatalogsResponse FindCatalogsNoAuthResponse { get; private set; }
            = new AdminCatalogsResponse()
            {
                Success = false,
                Message = "Seller not authenticated",
                Catalogs = null
            };

        // standard catalog response based upon the seed data
        public static AdminCatalogResponse CatalogResponse { get; private set; }
            = new AdminCatalogResponse()
            {
                Success = true,
                Message = null,
                Catalog = new WebStoreCatalog()
                {
                    Id = 1,
                    Name = "TEST_CATALOG",
                    Description = "TEST_CATALOG description"
                }
            };

        // "Catalog not found"
        public static AdminCatalogResponse CatalogNotFoundResponse { get; private set; }
            = new AdminCatalogResponse()
            {
                Success = false,
                Message = "Catalog not found",
                Catalog = null
            };

        // "Seller not authenticated"
        public static AdminCatalogResponse CatalogNoAuthResponse { get; private set; }
            = new AdminCatalogResponse()
            {
                Success = false,
                Message = "Seller not authenticated",
                Catalog = null
            };

        // standard catalog response based upon updated seed data
        public static AdminCatalogResponse CatalogUpdateResponse { get; private set; }
            = new AdminCatalogResponse()
            {
                Success = true,
                Message = null,
                Catalog = new WebStoreCatalog()
                {
                    Id = 1,
                    Name = "TEST_CATALOG",
                    Description = "TEST_CATALOG updated description"
                }
            };

        // "Seller Id mismatch"
        public static AdminCatalogResponse CatalogSellerIdMismatchResponse { get; private set; }
            = new AdminCatalogResponse()
            {
                Success = false,
                Message = "There was a problem saving the catalog",
                Catalog = null
            };

        #endregion

        #region Product Responses

        // standard product response based upon the seed data
        public static AdminProductResponse ProductResponse { get; private set; }
            = new AdminProductResponse()
            {
                Success = true,
                Message = null,
                Product = new Product()
                {
                    Id = 3,
                    CatalogId = 2,
                    SellerProductId = null,
                    Name = "TEST_PRODUCT",
                    Summary = "TEST_PRODUCT summary",
                    Detail = "TEST_PRODUCT detail",
                    Price = 5.00m,
                    IsDownloadable = false, // want to be sure we are testing for non-default values
                    IsAvailable = false, // want to be sure we are testing for non-default values
                    SupplierName = "TEST_PRODUCT supplier name",
                    ShippingWeight = 1.00m
                }
            };

        // "Product not found"
        public static AdminProductResponse ProductNotFoundResponse { get; private set; }
            = new AdminProductResponse()
            {
                Success = false,
                Message = "Product not found",
                Product = null
            };

        // "Seller not authenticated"
        public static AdminProductResponse ProductNoAuthResponse { get; private set; }
            = new AdminProductResponse()
            {
                Success = false,
                Message = "Seller not authenticated",
                Product = null
            };

        // standard product response based upon updated seed data
        public static AdminProductResponse ProductUpdatedResponse { get; private set; }
            = new AdminProductResponse()
            {
                Success = true,
                Message = null,
                Product = new Product()
                {
                    Id = 3,
                    CatalogId = 2,
                    SellerProductId = null,
                    Name = "TEST_PRODUCT",
                    Summary = "TEST_PRODUCT updated summary",
                    Detail = "TEST_PRODUCT detail",
                    Price = 5.00m,
                    IsDownloadable = false, // want to be sure we are testing for non-default values
                    IsAvailable = false, // want to be sure we are testing for non-default values
                    SupplierName = "TEST_PRODUCT supplier name",
                    ShippingWeight = 1.00m
                }


            };
        
        // "Catalog Id mismatch"
        public static AdminProductResponse ProductCatalogIdMismtachResponse { get; private set; }
            = new AdminProductResponse()
            {
                Success = false,
                Message = "There was a problem saving the product",
                Product = null
            };

        #endregion
    }
}
