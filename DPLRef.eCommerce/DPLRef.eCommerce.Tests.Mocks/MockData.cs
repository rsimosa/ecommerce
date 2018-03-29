using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockData
    {
        public static readonly Guid MySessionId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        public static readonly Guid MyBadSessionId = new Guid("11111111-bbbb-cccc-dddd-eeeeeeeeeeee");
        public static readonly Guid MyBothInfoSessionId = new Guid("ffffffff-bbbb-cccc-dddd-eeeeeeeeeeee");
        public static readonly Guid MySessionIdForOrder = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeef");

        public static readonly Address MyAddress = new Address()
        {
            Addr1 = "4771 Snowbird Lane",
            Addr2 = "",
            City = "Hooper",
            First = "Neil",
            Last = "Diaz",
            Postal = "68512",
            State = "Nebraska"
        };
        public static readonly Address MySameAddress = new Address()
        {
            Addr1 = "2498 Post Avenue",
            Addr2 = "",
            City = "Lincoln",
            First = "Jeanine",
            Last = "Jeanine M Gilbert",
            Postal = "68512",
            State = "Nebraska"
        };
        public static readonly Address MyBadAddress = new Address();

        public AmbientContext Context { get; set; }

        public bool OrderCreated;
        public bool OrderSucceeded;
        public bool CartDeleted;
        public bool AsyncCalled { get; set; }
        public bool ForceException { get; set; }        
        public bool ForceCaptureFail { get; set; }
        
        public bool OrderCaptureAttempted { get; set; }
        public bool OrderCapturedStatus { get; set; }
        public bool OrderShippingRequested { get; set; }
        public bool ForceShippingFail { get; set; }
        public bool OrderFulfilled { get; set; }
        public Order OrderToFulfill { get; set; }


        public List<WebStoreCatalog> Catalogs { get; set; } = new List<WebStoreCatalog>
        {
            new WebStoreCatalog()
            {
                Id = 1,
                Name = "My Webstore"
            },
            new WebStoreCatalog()
            {
                Id = 2,
                Name = "My Second Webstore"
            }
        };

        public List<Product> Products { get; set; } = new List<Product>
        {
            new Product()
            {
                Id = 1,
                Name = "My Product",
                Summary = "My Product Summary",
                CatalogId = 1,
                Price = 1.50m
            },
            new Product()
            {
                Id = 2,
                Name = "My Second Product",
                Summary = "My Second Product Summary",
                CatalogId = 1,
            }
        };

        public List<Cart> Carts { get; set; } = new List<Cart>
        {
            new Cart()
            {
                Id = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                CartItems = new CartItem[]
                {
                }
            },
            new Cart()
            {
                Id = MySessionIdForOrder,
                BillingAddress = MyAddress,
                ShippingAddress = MyAddress,
                CartItems = new CartItem[]
                {
                    new CartItem()
                    {
                        ProductId = 1,
                        ProductName = "Mock Product Name",
                        Quantity = 1,
                    }
                }
            }
        };

        public List<Order> Orders { get; set; } = new List<Order>
        {
        };

        
        public List<Seller> Sellers { get; set; } = new List<Seller>()
        {
            new Seller()
            {
                Id = 1,
                Name = "Test Seller"
            }
        };

        public SellerSalesTotal SellerSalesTotal = new SellerSalesTotal()
        {
            OrderCount = 1,
            OrderTotal = 10.0m
        };

        public List<SellerSalesTotal> SellerSalesTotaList { get; set; } = new List<SellerSalesTotal>
        {
            new SellerSalesTotal()
            {
            SellerId = 1,
            SellerName = "UNIT TEST",
            OrderCount = 1,
            OrderTotal = 10.0M,
            }
        };

        public List<SellerOrderData> SellerOrderData { get; set; } = new List<SellerOrderData>
        {
            new SellerOrderData()
            {
                SellerId = 1,
                SellerName = "UNIT TEST",
                OrderCount = 1,
                OrderTotal = 10.0M,
            }
        };

        public ShippingResult ShippingResult { get; set; } = new ShippingResult()
        {
            Success = false,
            ShippingProvider = "",
            TrackingCode = ""
        };
    }
}
