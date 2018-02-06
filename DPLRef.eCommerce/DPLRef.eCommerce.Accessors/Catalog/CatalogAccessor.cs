using System;
using System.Linq;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using Product = DPLRef.eCommerce.Accessors.DataTransferObjects.Product;
using DPLRef.eCommerce.Accessors.EntityFramework;
using AutoMapper.QueryableExtensions;

namespace DPLRef.eCommerce.Accessors.Catalog
{

    class CatalogAccessor : AccessorBase, ICatalogAccessor
    {

        public WebStoreCatalog Find(int catalogId)
        {
            using (var db = DbContextFactory.Create())
            {
                var catalogExtended = (from c in db.Catalogs
                                       join s in db.Sellers on c.SellerId equals s.Id
                                       where c.Id == catalogId
                                       select new CatalogExtended { Catalog = c, SellerName = s.Name }).FirstOrDefault();

                if (catalogExtended != null)
                {
                    // Using a special Map method for handling the combination of Catalog and Seller fields
                    return DTOMapper.Map(catalogExtended);
                }
                return null;
            }
        }

        public WebStoreCatalog SaveCatalog(WebStoreCatalog catalog)
        {
            // Map the seller id in the ambient context to the catalog parameter
            if (catalog.SellerId == 0)
                catalog.SellerId = Context.SellerId;

            using (var db = DbContextFactory.Create())
            {
                EntityFramework.Catalog model = null;
                if (catalog.Id > 0)
                {
                    model = db.Catalogs.Find(catalog.Id);
                    // verify the db catalog belongs to the current seller and matches the catalog input seller
                    if (model != null && (model.SellerId != catalog.SellerId || model.SellerId != Context.SellerId))
                    {
                        Logger.Error("Seller Id mismatch");
                        throw new ArgumentException("Seller Id mismatch");
                    }
                    DTOMapper.Map(catalog, model);
                }
                else
                {
                    model = new EntityFramework.Catalog();
                    DTOMapper.Map(catalog, model);
                    db.Catalogs.Add(model);

                }
                db.SaveChanges();

                return Find(model.Id);
            }
        }

        public void DeleteCatalog(int id)
        {
            using (var db = DbContextFactory.Create())
            {
                var model = db.Catalogs.Find(id);
                if (model != null && model.SellerId == Context.SellerId) // make sure the seller context matches the model seller
                {
                    db.Catalogs.Remove(model);
                    db.SaveChanges();
                }
                else
                {
                    Logger.Error("Seller Id mismatch");
                    throw new ArgumentException("Seller Id mismatch");
                }
            }
        }

        public WebStoreCatalog[] FindAllSellerCatalogs()
        {
            using (var db = DbContextFactory.Create())
            {
                return db.Catalogs
                         .Where(c => c.SellerId == Context.SellerId)
                         .ProjectTo<WebStoreCatalog>(DTOMapper.Configuration)
                         .ToArray();
            }
        }

        public Product[] FindAllProductsForCatalog(int catalogId)
        {
            using (var db = DbContextFactory.Create())
            {
                return db.Products
                         .Where(p => p.CatalogId == catalogId)
                         .ProjectTo<Product>(DTOMapper.Configuration)
                         .ToArray();
            }
        }

        public Product FindProduct(int id)
        {
            using (var db = DbContextFactory.Create())
            {
                return db.Products
                         .Where(p => p.Id == id)
                         .ProjectTo<Product>(DTOMapper.Configuration)
                         .FirstOrDefault();
            }
        }

        public Product SaveProduct(int catalogId, Product product)
        {
            // map the catalog id in the ambient context to the product parameter
            if (product.CatalogId == 0)
                product.CatalogId = catalogId;

            using (var db = DbContextFactory.Create())
            {
                EntityFramework.Product model = null;
                if (product.Id > 0)
                {
                    model = db.Products.Find(product.Id);
                    if (model == null)
                    {
                        Logger.Error("Product Id not found");
                        throw new ArgumentException("Product Id not found");
                    }
                    // verify the db product belongs to the current catalog and matches the product input catalog
                    if (model != null && (model.CatalogId != product.CatalogId || model.CatalogId != catalogId))
                    {
                        Logger.Error("Catalog Id mismatch");
                        throw new ArgumentException("Catalog Id mismatch");
                    }
                    DTOMapper.Map(product, model);
                }
                else
                {
                    model = new EntityFramework.Product();
                    DTOMapper.Map(product, model);
                    db.Products.Add(model);
                }
                db.SaveChanges();

                return DTOMapper.Map<Product>(model);
            }
        }

        public void DeleteProduct(int catalogId, int id)
        {
            using (var db = DbContextFactory.Create())
            {
                var model = db.Products.Find(id);
                if (model != null && model.CatalogId == catalogId) // make sure the Catalog context matches the model Catalog
                {
                    db.Products.Remove(model);
                    db.SaveChanges();
                }
                else
                {
                    Logger.Error("Catalog Id mismatch");
                    throw new ArgumentException("Catalog Id mismatch");
                }
            }
        }
    }
}
