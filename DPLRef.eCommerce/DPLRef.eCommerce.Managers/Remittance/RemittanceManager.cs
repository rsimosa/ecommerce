using System;
using System.Collections.Generic;
using DPLRef.eCommerce.Accessors.Remittance;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Engines.Remmitance;
using DPLRef.eCommerce.Utilities;
using BackOffice = DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance;
using Admin = DPLRef.eCommerce.Contracts.Admin.Sales;

namespace DPLRef.eCommerce.Managers.Remittance
{
    class RemittanceManager : ManagerBase, BackOffice.IBackOfficeRemittanceManager, Admin.IAdminRemittanceManager
    {
        #region IServiceContractBase
        public override string TestMe(string input)
        {
            input = base.TestMe(input);
            input = AccessorFactory.CreateAccessor<IRemittanceAccessor>().TestMe(input);

            return input;
        }

        #endregion  

        #region BackOffice.IBackOfficeRemittanceManager

        BackOffice.OrderDataResponse BackOffice.IBackOfficeRemittanceManager.Totals()
        {
            try
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().BackOfficeAdminAuthenticated())
                {
                    var sellerOrderData = AccessorFactory.CreateAccessor<IRemittanceAccessor>()
                                                  .SalesTotal();

                    if (sellerOrderData != null && sellerOrderData.Length > 0)
                    {
                        var result = new BackOffice.OrderDataResponse();
                        var items = new List<BackOffice.SellerOrderData>();

                        foreach (var item in sellerOrderData)
                        {
                            var mapped = DTOMapper.Map<BackOffice.SellerOrderData>(item);

                            var calcResult = EngineFactory.CreateEngine<IRemittanceCalculationEngine>()
                                .CalculateFee(mapped.SellerId, mapped.OrderTotal);

                            mapped.FeeAmount = calcResult.FeeAmount;
                            mapped.RemittanceAmount = calcResult.RemittanceAmount;
                           
                            items.Add(mapped);
                        }

                        result.Success = true;
                        result.SellerOrderData = items.ToArray();
                        return result;
                    }
                    else
                    {
                        return new BackOffice.OrderDataResponse()
                        {
                            Success = false,
                            Message = "No orders"
                        };
                    }

                }
                return new BackOffice.OrderDataResponse()
                {
                    Success = false,
                    Message = "BackOfficeAdmin not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new BackOffice.OrderDataResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing sellers orders"
                };
            }
        }

        #endregion

        #region Admin.IAdminRemittanceManager

        public Admin.SalesTotalsResponse Totals()
        {
            try
            {
                // authenticate the user as a seller
                if (UtilityFactory.CreateUtility<ISecurityUtility>().SellerAuthenticated())
                {
                    var sellerSalesTotals = AccessorFactory.CreateAccessor<IOrderAccessor>()
                                                  .SalesTotal();
                    if (sellerSalesTotals != null)
                    {
                        return new Admin.SalesTotalsResponse()
                        {
                            Success = true,
                            OrderCount = sellerSalesTotals.OrderCount,
                            OrderTotal = sellerSalesTotals.OrderTotal
                        };
                    }
                    else
                    {
                        return new Admin.SalesTotalsResponse()
                        {
                            Success = false,
                            Message = "No orders for this Seller"
                        };
                    }

                }
                return new Admin.SalesTotalsResponse()
                {
                    Success = false,
                    Message = "Seller not authenticated"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Admin.SalesTotalsResponse()
                {
                    Success = false,
                    Message = "There was a problem accessing this seller's orders"
                };
            }
        }

        #endregion
        

    }
}
