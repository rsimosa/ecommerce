using DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Common.Shared;
using Admin = DPLRef.eCommerce.Contracts.Admin.Catalog;
using WebStore = DPLRef.eCommerce.Contracts.WebStore.Catalog;
using System;
using System.Collections.Generic;
using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Managers.Catalog
{
    class CatalogManager : ManagerBase, WebStore.IWebStoreCatalogManager, Admin.IAdminCatalogManager
    {

        #region IAdminCatalogManager

        Admin.AdminCatalogsResponse Admin.IAdminCatalogManager.FindCatalogs()
        {
            try
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    var catalogs = AccessorFactory.CreateAccessor<ICatalogAccessor>()
                        .FindAllSellerCatalogs();

                    List<Admin.WebStoreCatalog> catalogList = new List<Admin.WebStoreCatalog>();
                    foreach (var catalog in catalogs)
                    {
                        Admin.WebStoreCatalog result = new Admin.WebStoreCatalog();
                        DTOMapper.Map(catalog, result);
                        catalogList.Add(result);
                    }
                    return new Admin.AdminCatalogsResponse()
                    {
                        Success = true,
                        Catalogs = catalogList.ToArray()
                    };

                }
                return new Admin.AdminCatalogsResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.AdminCatalogsResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing this seller's catalogs"
                };
            }
        }

        Admin.AdminCatalogResponse Admin.IAdminCatalogManager.SaveCatalog(Admin.WebStoreCatalog catalog)
        {
            try 
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    // map to the accessor DTO
                    WebStoreCatalog accCatalog = new WebStoreCatalog();
                    DTOMapper.Map(catalog, accCatalog);

                    accCatalog = AccessorFactory.CreateAccessor<ICatalogAccessor>().SaveCatalog(accCatalog);

                    if (accCatalog != null)
                    {
                        Admin.WebStoreCatalog result = new Admin.WebStoreCatalog();
                        DTOMapper.Map(accCatalog, result);

                        return new Admin.AdminCatalogResponse()
                        {
                            Success = true,
                            Catalog = result
                        };
                    }
                    return new Admin.AdminCatalogResponse()
                    {
                        Success = false,
                        Message = "Catalog not saved"
                    };
                }
                return new Admin.AdminCatalogResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.AdminCatalogResponse()
                {
                    Success = false,
                    Message = "There was a problem saving the catalog"
                };
            }
        }

        Admin.AdminProductResponse Admin.IAdminCatalogManager.SaveProduct(int catalogId, Admin.Product product)
        {
            try
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    // map to the accessor DTO
                    Product accProduct = new Product();
                    DTOMapper.Map(product, accProduct);

                    accProduct = AccessorFactory.CreateAccessor<ICatalogAccessor>().SaveProduct(catalogId, accProduct);

                    if (accProduct != null)
                    {
                        Admin.Product result = new Admin.Product();
                        DTOMapper.Map(accProduct, result);

                        return new Admin.AdminProductResponse()
                        {
                            Success = true,
                            Product = result
                        };
                    }
                    return new Admin.AdminProductResponse()
                    {
                        Success = false,
                        Message = "Product not saved"
                    };
                }
                return new Admin.AdminProductResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.AdminProductResponse()
                {
                    Success = false,
                    Message = "There was a problem saving the product"
                };
            }
        }

        Admin.AdminCatalogResponse Admin.IAdminCatalogManager.ShowCatalog(int catalogId)
        {
            try
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    var catalog = AccessorFactory.CreateAccessor<ICatalogAccessor>()
                        .Find(catalogId);

                    if (catalog != null)
                    {
                        if (catalog.SellerId == Context.SellerId)
                        {
                            Admin.WebStoreCatalog result = new Admin.WebStoreCatalog();
                            DTOMapper.Map(catalog, result);

                            return new Admin.AdminCatalogResponse()
                            {
                                Success = true,
                                Catalog = result
                            };
                        }
                    }
                    return new Admin.AdminCatalogResponse()
                    {
                        Success = false,
                        Message = "Catalog not found"
                    };
                }
                return new Admin.AdminCatalogResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.AdminCatalogResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing the catalog"
                };
            }
        }
  
        Admin.AdminProductResponse Admin.IAdminCatalogManager.ShowProduct(int catalogId, int productId)
        {
            try
            {
                //authenticate the seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    var product = AccessorFactory.CreateAccessor<ICatalogAccessor>()
                        .FindProduct(productId);

                    if (product != null)
                    {
                        if (product.CatalogId == catalogId)
                        {
                            Admin.Product result = new Admin.Product();
                            DTOMapper.Map(product, result);

                            return new Admin.AdminProductResponse()
                            {
                                Success = true,
                                Product = result
                            };
                        }
                    }
                    return new Admin.AdminProductResponse()
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }
                return new Admin.AdminProductResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.AdminProductResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing the product"
                };
            }
        }

        #endregion

        #region IWebStoreCatalogManager

        WebStore.WebStoreCatalogResponse WebStore.IWebStoreCatalogManager.ShowCatalog(int catalogId)
        {
            try
            {
                // Get the webstore catalog
                WebStore.WebStoreCatalog result = new WebStore.WebStoreCatalog();
                ICatalogAccessor catalogAccessor = AccessorFactory.CreateAccessor<ICatalogAccessor>();
                WebStoreCatalog accCatalog = catalogAccessor.Find(catalogId);

                // Get the webstore catalog products
                if (accCatalog != null)
                {
                    DTOMapper.Map(accCatalog, result);

                    Product[] catalogProducts = catalogAccessor.FindAllProductsForCatalog(catalogId);
                    List<WebStore.ProductSummary> productList = new List<WebStore.ProductSummary>();

                    foreach (var catalogProduct in catalogProducts)
                    {
                        WebStore.ProductSummary product = new WebStore.ProductSummary();
                        DTOMapper.Map(catalogProduct, product);
                        productList.Add(product);
                    }
                    result.Products = productList.ToArray();

                    return new WebStore.WebStoreCatalogResponse()
                    {
                        Success = true,
                        Catalog = result
                    };
                }
                return new WebStore.WebStoreCatalogResponse()
                {
                    Success = false,
                    Message = "Catalog not found"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new WebStore.WebStoreCatalogResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing the catalog"
                };
            }
        }

        WebStore.WebStoreProductResponse WebStore.IWebStoreCatalogManager.ShowProduct(int catalogId, int productId)
        {
            try
            {
                WebStore.ProductDetail result = new WebStore.ProductDetail();
                Product catProduct = AccessorFactory.CreateAccessor<ICatalogAccessor>().FindProduct(productId);

                if (catProduct != null)
                {
                    if (catProduct.CatalogId == catalogId)
                    {
                        DTOMapper.Map(AccessorFactory.CreateAccessor<ICatalogAccessor>().FindProduct(productId), result);

                        return new WebStore.WebStoreProductResponse()
                        {
                            Success = true,
                            Product = result
                        };
                    }
                }
                return new WebStore.WebStoreProductResponse()
                {
                    Success = false,
                    Message = "Product not found"
                };

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreProductResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing the product"
                };
            }
        }

        #endregion

        #region IServiceContractBase

        string IServiceContractBase.TestMe(string input)
        {
            input = base.TestMe(input);
            input = AccessorFactory.CreateAccessor<ICatalogAccessor>().TestMe(input);

            return input;
        }
        #endregion

    }
}
