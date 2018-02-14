using System;
using System.Collections.Generic;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using SalesAcc = DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Contracts.Admin.Sales;
using WebStore = DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Engines.Sales;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Managers.Sales
{
    class OrderManager : ManagerBase, WebStore.IWebStoreCartManager, WebStore.IWebStoreOrderManager, IReturnsManager, IAdminFulfillmentManager
    {
        #region IServiceContractBase

        public override string TestMe(string input)
        {
            input = base.TestMe(input);
            input = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>().TestMe(input);
            input = EngineFactory.CreateEngine<ITaxCalculationEngine>().TestMe(input);
            input = EngineFactory.CreateEngine<ICartPricingEngine>().TestMe(input);
            return input;
        }

        #endregion

        #region IWebStoreCartManager

        public WebStore.WebStoreCartResponse SaveCartItem(int catalogId, int productId, int quantity)
        {
            try
            {
                // NOTE: no need to check for cart id in context since the accessor will take care of this for us and create a cart if necessary
                SalesAcc.ICartAccessor cartAccessor = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>();

                // Save the cart item
                Cart storedCart = cartAccessor.SaveCartItem(catalogId, productId, quantity);

                // generate the pricing and taxes for the cart and convert to the public DTO
                var result = GenerateCartPricingAndTax(storedCart);

                // Return the cart
                return new WebStore.WebStoreCartResponse()
                {
                    Success = true,
                    Cart = result
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreCartResponse()
                {
                    Success = false,
                    Message = "There was a problem editting the cart"
                };
            }
        }

        public WebStore.WebStoreCartResponse RemoveCartItem(int catalogId, int productId)
        {
            // remove cart itme accomplished by call save cart item with quantity = 0
            return SaveCartItem(catalogId, productId, 0);
        }

        public WebStore.WebStoreCartResponse ShowCart(int catalogId)
        {
            try
            {
                // NOTE: no need to check for cart/session as the accessor will handle this
                // lookup the cart id
                Cart storedCart = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>().ShowCart(catalogId);
                if (storedCart != null)
                {
                    // generate the pricing and taxes for the cart and convert to the public DTO
                    var result = GenerateCartPricingAndTax(storedCart);

                    return new WebStore.WebStoreCartResponse()
                    {
                        Success = true,
                        Cart = result
                    };
                }
                // can't find the cart in the database
                return new WebStore.WebStoreCartResponse()
                {
                    Success = false,
                    Message = "Cart not found"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreCartResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing the cart"
                };
            }
        }

        private WebStore.WebStoreCart GenerateCartPricingAndTax(Cart storedCart)
        {
            // Generate the pricing for the cart (current product prices, coupon discounts, volume discounts
            ICartPricingEngine pricingEngine = EngineFactory.CreateEngine<ICartPricingEngine>();
            WebStore.WebStoreCart result = pricingEngine.GenerateCartPricing(storedCart);

            // Calculate taxes for the items in the cart
            ITaxCalculationEngine taxEngine = EngineFactory.CreateEngine<ITaxCalculationEngine>();
            result = taxEngine.CalculateCartTax(result);

            return result;
        }

        public WebStore.WebStoreCartResponse UpdateBillingInfo(int catalogId, Address billingInfo, bool shippingSameAsBilling)
        {
            try
            {
                // NOTE: no need to check for cart id in context since the accessor will take care of this for us and create a cart if necessary
                SalesAcc.ICartAccessor cartAccessor = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>();

                // Save the billing info
                Cart storedCart = cartAccessor.SaveBillingInfo(catalogId, billingInfo);

                // save the shipping info if the same
                if (shippingSameAsBilling)
                {
                    storedCart = cartAccessor.SaveShippingInfo(catalogId, billingInfo);
                }

                // generate the pricing and taxes for the cart and convert to the public DTO
                var result = GenerateCartPricingAndTax(storedCart);

                // Return the cart
                return new WebStore.WebStoreCartResponse()
                {
                    Success = true,
                    Cart = result
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreCartResponse()
                {
                    Success = false,
                    Message = "There was a problem saving the billing info"
                };
            }
        }

        public WebStore.WebStoreCartResponse UpdateShippingInfo(int catalogId, Address shippingInfo, bool billingSameAsShipping)
        {
            try
            {
                // NOTE: no need to check for cart id in context since the accessor will take care of this for us and create a cart if necessary
                SalesAcc.ICartAccessor cartAccessor = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>();

                // Save the billing info
                Cart storedCart = cartAccessor.SaveShippingInfo(catalogId, shippingInfo);

                // save the shipping info if the same
                if (billingSameAsShipping)
                {
                    storedCart = cartAccessor.SaveBillingInfo(catalogId, shippingInfo);
                }

                // generate the pricing and taxes for the cart and convert to the public DTO
                var result = GenerateCartPricingAndTax(storedCart);

                // Return the cart
                return new WebStore.WebStoreCartResponse()
                {
                    Success = true,
                    Cart = result
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreCartResponse()
                {
                    Success = false,
                    Message = "There was a problem saving the shipping info"
                };
            }
        }

        #endregion

        #region IWebStoreOrderManager

        public WebStore.WebStoreOrderResponse SubmitOrder(int catalogId, PaymentInstrument paymentInstrument)
        {
            try
            {       
                var result = new WebStore.WebStoreOrder();

                // Get the shopping cart
                SalesAcc.ICartAccessor cartAccessor = AccessorFactory.CreateAccessor<SalesAcc.ICartAccessor>();

                var storedCart = cartAccessor.ShowCart(catalogId);
                
                // make sure we have a valid cart to start
                if (storedCart != null)
                {

                    // Calculate the cart totals and tax
                    var fullCart = GenerateCartPricingAndTax(storedCart);

                    // create the order records
                    SalesAcc.IOrderAccessor orderAccessor = AccessorFactory.CreateAccessor<SalesAcc.IOrderAccessor>();

                    var submittedOrder = new Order();
                    DTOMapper.Map(fullCart, submittedOrder);

                    // validate the order
                    var validationEngine = EngineFactory.CreateEngine<IOrderValidationEngine>();
                    var validationResponse = validationEngine.ValidateOrder(submittedOrder);

                    if (validationResponse.Success)
                    {

                        submittedOrder.Status = OrderStatuses.Created;
                        var savedOrder = orderAccessor.SaveOrder(catalogId, submittedOrder);

                        // attempt the payment authorization if amount > 0
                        if (savedOrder.Total > 0)
                        {
                            SalesAcc.IPaymentAccessor paymentAccessor = AccessorFactory.CreateAccessor<SalesAcc.IPaymentAccessor>();
                            var authResult = paymentAccessor.Authorize(paymentInstrument, savedOrder.Id, savedOrder.Total);

                            if (authResult.Success)
                            {
                                // save the auth code for use in capture
                                savedOrder.AuthorizationCode = authResult.AuthCode;
                            }
                            else // problems with the authorization
                            {
                                // update the order status
                                savedOrder.Status = OrderStatuses.Failed;
                                savedOrder = orderAccessor.SaveOrder(catalogId, savedOrder);

                                // Return the order response
                                DTOMapper.Map(savedOrder, result);
                                return new WebStore.WebStoreOrderResponse()
                                {
                                    Success = false,
                                    Message = "There was a problem processing the payment",
                                    Order = result
                                };
                            }

                        }
                        // update the order status
                        savedOrder.Status = OrderStatuses.Authorized;
                        savedOrder = orderAccessor.SaveOrder(catalogId, savedOrder);


                        // Delete the cart once the order is successful
                        cartAccessor.DeleteCart(catalogId);

                        // send order submission event
                        UtilityFactory.CreateUtility<IAsyncUtility>().SendEvent(AsyncEventTypes.OrderSubmitted, savedOrder.Id);

                        // Return the order response
                        DTOMapper.Map(savedOrder, result);
                        return new WebStore.WebStoreOrderResponse()
                        {
                            Success = true,
                            Order = result
                        };
                    }
                    // order is not valid
                    return new WebStore.WebStoreOrderResponse()
                    {
                        Success = false,
                        Message = validationResponse.Message
                    };
                }
                // cart not found or not valid
                return new WebStore.WebStoreOrderResponse()
                {
                    Success = false,
                    Message = "Cart is not valid"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new WebStore.WebStoreOrderResponse()
                {
                    Success = false,
                    Message = "There was a problem processing the order"
                };
            }
        }

        #endregion

        #region Admin Fulfillment Manager

        public AdminOpenOrdersResponse GetOrdersToFulfill()
        {
            try
            {
                // authenticate the seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    // get the unfilfilled orders for the seller
                    var orders = AccessorFactory.CreateAccessor<SalesAcc.IOrderAccessor>().UnfulfilledOrders();

                    List<AdminUnfulfilledOrder> orderList = new List<AdminUnfulfilledOrder>();
                    foreach (var order in orders)
                    {
                        // map them to the contract dto and add to the list
                        AdminUnfulfilledOrder unfulfilledOrder = new AdminUnfulfilledOrder();
                        DTOMapper.Map(order, unfulfilledOrder);
                        orderList.Add(unfulfilledOrder);
                    }
                    return new AdminOpenOrdersResponse()
                    {
                        Success = true,
                        Orders = orderList.ToArray()
                    };
                }
                return new AdminOpenOrdersResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new AdminOpenOrdersResponse()
                {
                    Success = false,
                    Message = "There was a problem returning the unfulfilled orders"
                };
            }
        }

        public AdminFulfillmentResponse FulfillOrder(int orderId)
        {
            try
            {
                // authenticate the seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    var orderAccessor = AccessorFactory.CreateAccessor<SalesAcc.IOrderAccessor>();
                    var order = orderAccessor.FindOrder(orderId);
                    // Make sure the order is available and has a staus that is appropriate for fulfillment
                    if (order != null && order.Status == OrderStatuses.Authorized)
                    {
                        // capture the payment
                        var captureResult = AccessorFactory.CreateAccessor<SalesAcc.IPaymentAccessor>()
                                                .Capture(order.AuthorizationCode);

                        if (captureResult.Success)
                        {
                            // update the order status
                            order = orderAccessor.UpdateOrderStatus(orderId, OrderStatuses.Captured, "");

                            // notify the shipping provider
                            var shippingResult = AccessorFactory.CreateAccessor<SalesAcc.IShippingAccessor>().RequestShipping(order.Id);
                            if (shippingResult.Success)
                            {
                                // fulfill the order
                                order = orderAccessor.FulfillOrder(orderId, shippingResult.ShippingProvider, shippingResult.TrackingCode, "");

                                // send the fullfillment event
                                UtilityFactory.CreateUtility<IAsyncUtility>().SendEvent(AsyncEventTypes.OrderShipped, order.Id);

                                // return the result
                                return new AdminFulfillmentResponse()
                                {
                                    Success = true
                                };
                            }
                            //TODO: send a message to manual order management
                            //Update the order notes with message from shipping failure
                            order = orderAccessor.UpdateOrderStatus(orderId, order.Status, "Unable to request shipping for order");
                            return new AdminFulfillmentResponse()
                            {
                                Success = false,
                                Message = order.Notes
                            };
                        }
                        // TODO: send a message to manual order management
                        // Update the order notes with message from capture failure
                        order = orderAccessor.UpdateOrderStatus(orderId, order.Status, "Capture failed for authorization");
                        return new AdminFulfillmentResponse()
                        {
                            Success = false,
                            Message = order.Notes
                        };
                    }
                    // TODO: send a message to manual order management
                    return new AdminFulfillmentResponse()
                    {
                        Success = false,
                        Message = "Invalid order for fulfillment"
                    };
                }
                return new AdminFulfillmentResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new AdminFulfillmentResponse()
                {
                    Success = false,
                    Message = "There was a problem fulfilling the order"
                };
            }
        }

        #endregion
    }
}
