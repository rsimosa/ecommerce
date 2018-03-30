using System;

namespace DPLRef.eCommerce.Database
{
    interface ISeedDataAccessor : IDisposable
    {
        int CreateSeller(string username, string sellerName);
        int CreateCatalog(int sellerId, string catalogName);
        int CreateProduct(int catalogId, string name, bool isAvailable, bool isDownloadable, string summary, string detail, string supplier, decimal price);
    }
}
