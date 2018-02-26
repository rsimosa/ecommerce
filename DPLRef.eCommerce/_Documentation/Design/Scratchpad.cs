using System;

// DLL will be DPLRef.eCommerce.Contracts.Webstore
namespace DPLRef.eCommerce.Contracts.Webstore.Catalogs
{
    class Catalog
    {}

    class Product
    {}

    interface IWebstoreCatalogManager
    {
        Catalog ShowCatalog()
        {}

        Product ShowProduct()
        {}
    }
}


// DLL will be DPLRef.eCommerce.Contracts.Admin
namespace DPLRef.eCommerce.Contracts.Admin.Catalogs
{
    class Catalog
    { }

    class Product
    { }

    interface IAdminCatalogManager
    {
        Catalog[] FindCatalogs()
        {}

        Catalog ShowCatalog()
        {}

        void SaveCatalog()
        { }

        Product ShowProduct()
        {}

        void SaveProduct()
        { }

    }
}

// DLL will be DPLRef.eCommerce.Manager.Catalogs
namespace DPLRef.eCommerce.Manager.Catalogs
{
    using DPLRef.eCommerce.Manager.Webstore.Catalogs;
    using DPLRef.eCommerce.Manager.Admin.Catalogs;

    class CatalogManager : IWebstoreCatalogManager, IAdminCatalogManager
    {}

}

// DLL will be DPLRef.eCommerce.Accessor.Catalogs
// access or accessor
namespace DPLRef.eCommerce.Accessor.Catalogs
{
    class Catalog
    {}

    class Product
    {}

    interface ICatalogAccessor
    {}

    interface IProductAccessor
    {}

    class CatalogAccessor : ICatalogAccessor, IProductAccessor
    { }
}

namespace DPLRef.eCommerce.Accessor.Common
{
    class Order
    {}

    class Return
    {}

    interface IOrderAccessor
    {}

    interface IReturnAccessor
    {}

    class OrderAccessor : IOrderAccessor, IReturnAccessor
    { }
}