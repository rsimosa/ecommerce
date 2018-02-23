using System;
using System.Linq;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Accessors.Remittance
{
    class SellerAccessor : AccessorBase, ISellerAccessor
    {
        public Seller Find(int id)
        {
            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                var model = (from s in db.Sellers
                             where s.Id == id
                             select s)
                            .FirstOrDefault();

                if (model != null)
                {
                    var seller = new Seller();
                    DTOMapper.Map(model, seller);
                    return seller;
                }
            }
            return null;
        }

        public Seller Save(Seller seller)
        {
            EntityFramework.Seller model = null;
            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                if (seller.Id > 0)
                {
                    model = db.Sellers.Find(seller.Id);
                    if (model == null)
                    {
                        // this state should never happen
                        throw new ArgumentException($"Trying to update Seller ({seller.Id}) that does not exist");
                    }
                    DTOMapper.Map(seller, model);
                }
                else
                {
                    model = new EntityFramework.Seller();
                    DTOMapper.Map(seller, model);
                    db.Sellers.Add(model);
                }
                db.SaveChanges();
            }

            if (model != null)
                return Find(model.Id);
            return null;
        }

        public void Delete(int id)
        {
            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                var model = (from s in db.Sellers
                             where s.Id == id
                             select s)
                            .FirstOrDefault();

                if (model != null)
                {
                    db.Sellers.Remove(model);
                    db.SaveChanges();
                }
            }
        }
    }
}
