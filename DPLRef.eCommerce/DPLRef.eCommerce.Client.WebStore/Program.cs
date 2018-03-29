using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Managers;
using System;
using DPLRef.eCommerce.Contracts.WebStore.Catalog;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Common.Contracts;


namespace DPLRef.eCommerce.Client.WebStore
{
    public class Program
    {
        private static Guid _sessionId = Guid.NewGuid();
        private static readonly Address _billingAddress = new Address()
        {
            Addr1 = "151 N 8th Street",
            Addr2 = "",
            City = "Lincoln",
            First = "Bob",
            Last = "Smith",
            EmailAddress = "my.email.address@example.com",
            Postal = "68508",
            State = "Nebraska"
        };
        private static readonly Address _shippingAddress = new Address()
        {
            Addr1 = "151 N 8th Street",
            Addr2 = "",
            City = "Lincoln",
            First = "Bob",
            Last = "Smith",
            EmailAddress = "my.email.address@example.com",
            Postal = "68508",
            State = "Nebraska"
        };

        private static readonly PaymentInstrument PaymentInstrument = new PaymentInstrument()
        {
            AccountNumber = "4000111122223333",
            VerificationCode = 123,
            ExpirationDate = "10/20",
            PaymentType = PaymentTypes.CreditCard
        };


        static void Main()
        {
            Console.WriteLine("Starting WebStore");

            bool exitApp = false;
            // get the first menu selection
            int menuSelection = InitConsoleMenu();

            while (menuSelection != 99)
            {
                switch (menuSelection)
                {
                    case 0:
                        Console.WriteLine("Please enter a valid menu selection.");
                        Console.WriteLine();
                        break;
                    case 1: // Show the catalog
                        ShowCatalog(1);
                        break;
                    case 2: // Show catalog not found
                        ShowCatalog(-1);
                        break;
                    case 3: // Show product detail
                        ShowProductDetail(1, 1);
                        break;
                    case 4: // Show product not found
                        ShowProductDetail(1, -1);
                        break;
                    case 5: // Add product to cart
                        AddItemToCart(1, 1, _sessionId);
                        break;
                    case 6: // Add additional product to an existing cart
                        AddItemToCart(2, 1, _sessionId);
                        break;
                    case 7: // Increase the quantity of an item
                        AddItemToCart(1, 5, _sessionId);
                        break;
                    case 8: // Show the cart
                        ShowCart(_sessionId);
                        break;
                    case 9: // Show cart not found
                        ShowCart(Guid.NewGuid());
                        break;
                    case 10: // Remove item from cart
                        RemoveItemFromCart(1, _sessionId);
                        break;
                    case 11: // Remove item from cart not found
                        RemoveItemFromCart(1, Guid.NewGuid());
                        break;
                    case 12: // Update just the billing info
                        UpdateBillingInfo(_billingAddress, false, _sessionId);
                        break;
                    case 13: // Update just the shipping info
                        UpdateShippingInfo(_shippingAddress, false, _sessionId);
                        break;
                    case 14: // Set shipping == billing
                        UpdateBillingInfo(_billingAddress, true, _sessionId);
                        break;
                    case 15: // Set billing == shipping
                        UpdateShippingInfo(_shippingAddress, true, _sessionId);
                        break;
                    case 16: // Submit Order
                        SubmitOrder(_sessionId);
                        break;
                    case 99:
                        exitApp = true;
                        break;
                }

                // check to see if we want to exit the app
                if (exitApp)
                {
                    break; // exit the while loop
                }

                // re-initialize the menu selection
                menuSelection = InitConsoleMenu();
            }
        }

        private static int InitConsoleMenu()
        {
            int result;

            Console.WriteLine("Select desired option:");
            Console.WriteLine("Catalog ==================");
            Console.WriteLine(" 1: Show the catalog");
            Console.WriteLine(" 2: Show catalog not found");
            Console.WriteLine(" 3: Show product details");
            Console.WriteLine(" 4: Show product not found");
            Console.WriteLine("Cart =====================");
            Console.WriteLine(" 5: Add a product to cart");
            Console.WriteLine(" 6: Add additional product to cart");
            Console.WriteLine(" 7: Increase the quantity of an item in the cart");
            Console.WriteLine(" 8: Show the cart");
            Console.WriteLine(" 9: Show cart not found");
            Console.WriteLine(" 10: Remove item from cart");
            Console.WriteLine(" 11: Remove item from cart not found");
            Console.WriteLine(" 12: Update billing address in cart");
            Console.WriteLine(" 13: Update shipping address in cart");
            Console.WriteLine(" 14: Set shipping same as billing");
            Console.WriteLine(" 15: Set billing same as shipping");
            Console.WriteLine("Order =====================");
            Console.WriteLine(" 16: Submit Order");
            Console.WriteLine("Exit =====================");
            Console.WriteLine(" 99: exit");
            string selection = Console.ReadLine();
            if (int.TryParse(selection, out result) == false)
            {
                result = 0;
            }

            return result;
        }

        private static void ShowResponse(ResponseBase response, String result)
        {
            Console.WriteLine($"Result: {response.Success}");
            Console.WriteLine($"Message: {response.Message}");
            Console.WriteLine(result);
        }

        private static void ShowCatalog(int catalogId)
        {
            var context = new AmbientContext() { SellerId = 1 };
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IWebStoreCatalogManager>();
            var response = webStoreCatalogManager.ShowCatalog(catalogId);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCatalog>(response.Catalog));
        }

        private static void ShowProductDetail(int catalogId, int productId)
        {
            var context = new AmbientContext() { SellerId = 1};
            var managerFactory = new ManagerFactory(context);
            var webStoreCatalogManager = managerFactory.CreateManager<IWebStoreCatalogManager>();
            var response = webStoreCatalogManager.ShowProduct(catalogId, productId);
            ShowResponse(response, StringUtilities.DataContractToJson<ProductDetail>(response.Product));
        }

        private static void AddItemToCart(int productId, int quantity, Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreCartManager = managerFactory.CreateManager<IWebStoreCartManager>();
            var response = webStoreCartManager.SaveCartItem(1, productId, quantity);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCart>(response.Cart));
        }

        private static void ShowCart(Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreCartManager = managerFactory.CreateManager<IWebStoreCartManager>();
            var response = webStoreCartManager.ShowCart(1);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCart>(response.Cart));
        }

        private static void RemoveItemFromCart(int productId, Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreCartManager = managerFactory.CreateManager<IWebStoreCartManager>();
            var response = webStoreCartManager.RemoveCartItem(1, productId);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCart>(response.Cart));
        }

        private static void UpdateBillingInfo(Address billingAddress, bool shippingSameAsBilling, Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreCartManager = managerFactory.CreateManager<IWebStoreCartManager>();
            var response = webStoreCartManager.UpdateBillingInfo(1, billingAddress, shippingSameAsBilling);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCart>(response.Cart));
        }

        private static void UpdateShippingInfo(Address shippingAddress, bool billingSameAsShipping, Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreCartManager = managerFactory.CreateManager<IWebStoreCartManager>();
            var response = webStoreCartManager.UpdateShippingInfo(1, shippingAddress, billingSameAsShipping);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCart>(response.Cart));
        }

        private static void SubmitOrder(Guid sessionId)
        {
            var context = new AmbientContext() { SellerId = 1, SessionId = sessionId };
            var managerFactory = new ManagerFactory(context);
            var webStoreOrderManager = managerFactory.CreateManager<IWebStoreOrderManager>();
            var response = webStoreOrderManager.SubmitOrder(1, PaymentInstrument);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreOrder>(response.Order));
        }
    }
}
