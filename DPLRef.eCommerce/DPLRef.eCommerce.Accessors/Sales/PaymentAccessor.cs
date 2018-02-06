using System;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.Sales
{
    class PaymentAccessor : AccessorBase, IPaymentAccessor
    {
        public PaymentAuthStatusResult Authorize(PaymentInstrument paymentInstrument, int orderId, decimal total)
        {
            // if cc number starts with a 5, lets fail.
            if (!string.IsNullOrEmpty(paymentInstrument.AccountNumber) && paymentInstrument.AccountNumber[0] == '5')
                return new PaymentAuthStatusResult()
                {
                    Success = false,
                    ErrorCode = 1,
                    ErrorMessage = "Unit Test Mock Error"
                };

            // else return success
            return new PaymentAuthStatusResult()
            {
                Success = true,
                AuthCode = Guid.NewGuid().ToString() // use guid for auth code 
            };
        }

        public PaymentCaptureResult Capture(string authorizationCode)
        {
            // if auth code == "invalid_auth_code", lets fail.
            if (!string.IsNullOrEmpty(authorizationCode) && authorizationCode == "invalid_auth_code")
                return new PaymentCaptureResult()
                {
                    Success = false,
                    ErrorCode = 1,
                    ErrorMessage = "Unit Test Mock Error"
                };

            // else return success
            return new PaymentCaptureResult()
            {
                Success = true 
            };
        }
    }
}
