using DPLRef.eCommerce.Accessors.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockCatalogAccessor : MockBase, ICatalogAccessor
    {
        public MockCatalogAccessor(MockData data) : base(data)
        {

        }

        public void DeleteCatalog(int id)
        {
            var catalog = MockData.Catalogs.FirstOrDefault(c => c.Id == id);
            MockData.Catalogs.Remove(catalog);
        }

        public void DeleteProduct(int catalogId, int id)
        {
            var product = MockData.Products.FirstOrDefault(p => p.Id == id);
            MockData.Products.Remove(product);
        }

        public WebStoreCatalog Find(int catalogId)
        {
            if (catalogId == 99)
            {
                throw new System.Exception();
            }
            return MockData.Catalogs.FirstOrDefault(p => p.Id == catalogId);
        }

        public Product[] FindAllProductsForCatalog(int catalogId)
        {
            return MockData.Products.Where(p => p.CatalogId == catalogId).ToArray();
        }

        public WebStoreCatalog[] FindAllSellerCatalogs()
        {
            return MockData.Catalogs.ToArray();
        }

        public Product FindProduct(int id)
        {
            return MockData.Products.FirstOrDefault(p => p.Id == id);
        }

        public WebStoreCatalog SaveCatalog(WebStoreCatalog catalog)
        {
            if (catalog.Id == 0)
            {
                if (MockData.Catalogs.Count > 0)
                    catalog.Id = MockData.Catalogs.Max(x => x.Id) + 1;
                else
                    catalog.Id = 1;
            }

            var found = MockData.Catalogs.FirstOrDefault(x => x.Id == catalog.Id);
            if (found != null)
            {
                MockData.Catalogs.Remove(found);                
            }
            MockData.Catalogs.Add(catalog);

            return catalog;
        }

        public Product SaveProduct(int catalogId, Product product)
        {
            if (product.Id == 0)
            {
                product.Id = MockData.Products.Max(x => x.Id) + 1;
            }

            var found = MockData.Products.FirstOrDefault(x => x.Id == product.Id);
            if (found != null)
            {
                MockData.Products.Remove(found);
                MockData.Products.Add(product);
            }

            return product;
        }

        public string TestMe(string input)
        {
            return input;
        }
    }
}
