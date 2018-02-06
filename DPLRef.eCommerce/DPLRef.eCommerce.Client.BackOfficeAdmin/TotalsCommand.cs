using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.BackOfficeAdmin
{
    class TotalsCommand : BaseUICommand
    {
        public TotalsCommand(AmbientContext ambientContext)
            : base(ambientContext)
        {
        }

        public override string Name => "Sales Totals";


        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[] { };
        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var remittanceManager = managerFactory.CreateManager<IBackOfficeRemittanceManager>();

            var response = remittanceManager.Totals();

            if (response.Success)
                ShowResponse(response, StringUtilities.DataContractToJson<SellerOrderData[]>(response.SellerOrderData));
            else
                ShowResponse(response, response.Message);
        }
    }
}
