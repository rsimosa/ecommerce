using System;
using DPLRef.eCommerce.Common.Contracts;
using System.Collections.Generic;
using System.Linq;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Utilities;


namespace DPLRef.eCommerce.Accessors.Sales
{

    class CartAccessor : AccessorBase, ICartAccessor
    {
        public Cart ShowCart(int catalogId)
        {
            return FindCart(catalogId, Context.SessionId);
        }


        public Cart SaveBillingInfo(int catalogId, Address billingAddress)
        {
            EntityFramework.Cart model = null;

            // cannot create a cart without a session id
            if (Context.SessionId == Guid.Empty)
            {
                return null;
            }
            
            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                model = db.Carts.Find(Context.SessionId);

                if (model == null)
                {
                    model = new EntityFramework.Cart();
                    db.Carts.Add(model);
                }

                DTOMapper.MapBilling(billingAddress, model);
                model.CatalogId = catalogId;
                model.Id = Context.SessionId;

                db.SaveChanges();
            }

            return FindCart(catalogId, model.Id);
        }

        public Cart SaveShippingInfo(int catalogId, Address shippingAddress)
        {
            EntityFramework.Cart model = null;

            // cannot create a cart without a session id
            if (Context.SessionId == Guid.Empty)
            {
                return null;
            }

            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {

                model = db.Carts.Find(Context.SessionId);

                if (model == null)
                {
                    model = new EntityFramework.Cart();
                    db.Carts.Add(model);
                }

                DTOMapper.MapShipping(shippingAddress, model);
                model.CatalogId = catalogId;
                model.Id = Context.SessionId;

                db.SaveChanges();
            }
            return FindCart(catalogId, model.Id);
        }

        public Cart SaveCartItem(int catalogId, int productId, int quantity)
        {
            EntityFramework.Cart model = null;

            // cannot create a cart without a session id
            if (Context.SessionId == Guid.Empty)
            {
                return null;
            }

            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                model = db.Carts.Find(Context.SessionId);

                if (model == null)
                {
                    model = new EntityFramework.Cart();
                    model.CatalogId = catalogId;
                    model.Id = Context.SessionId;
                    db.Set<EntityFramework.Cart>().Add(model);
                    db.SaveChanges();
                }

                var cartItemModel = (from ci in db.CartItems where ci.CartId == Context.SessionId && ci.ProductId == productId select ci).FirstOrDefault();

                if (quantity == 0)
                {
                    if (cartItemModel != null)
                    {
                        // we need to remove this item
                        db.CartItems.Remove(cartItemModel);
                    }
                }
                else if (cartItemModel == null)
                {
                    // we need to add this item
                    cartItemModel = new EntityFramework.CartItem();
                    cartItemModel.CartId = model.Id;
                    cartItemModel.CatalogId = catalogId;
                    cartItemModel.ProductId = productId;
                    db.CartItems.Add(cartItemModel);
                }

                if (cartItemModel != null)
                    cartItemModel.Quantity = quantity;

                db.SaveChanges();
            }
            return FindCart(catalogId, model.Id);
        }

        private Cart FindCart(int catalogId, Guid id)
        {
            Cart cart = null;
            if (id != Guid.Empty)
            {
                using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
                {
                    EntityFramework.Cart model = db.Carts.Find(id);

                    // gracefully handle situation where the cart id does not exist
                    // or there is a catalog id mismatch
                    if (model != null && model.CatalogId == catalogId)
                    {
                        cart = DTOMapper.Map<Cart>(model);

                        var cartItemModels = from ci in db.CartItems
                                             join p in db.Products on ci.ProductId equals p.Id
                                             where ci.CartId == cart.Id
                                             select new { Model = ci, Name = p.Name };
                        
                        var cartItems = new List<CartItem>();

                        foreach (var cim in cartItemModels)
                        {
                            var cartitem = DTOMapper.Map<CartItem>(cim.Model);
                            cartitem.ProductName = cim.Name;
                            cartItems.Add(cartitem);
                        }

                        cart.CartItems = cartItems.ToArray();
                    }
                }
            }
            return cart;
        }

        public bool DeleteCart(int catalogId)
        {
            bool result = false;

            var sessionId = Context.SessionId;
            if (sessionId != Guid.Empty)
            {
                using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
                {
                    EntityFramework.Cart model = db.Carts.Find(sessionId);
                    // gracefully handle situation where the cart id does not exist
                    // or there is a catalog id mismatch
                    if (model != null && model.CatalogId == catalogId)
                    {
                        var cartItems = db.CartItems.Where(ci => ci.CartId == sessionId);

                        if (cartItems.Any())
                        {
                            foreach (var item in cartItems)
                            {
                                db.CartItems.Remove(item);
                            }
                            db.SaveChanges(); // delete so FK allows delete of carts
                        }

                        db.Carts.Remove(model);
                    }

                    db.SaveChanges();
                    result = true; // we deleted a cart
                }
            }

            return result;
        }
    }
}
