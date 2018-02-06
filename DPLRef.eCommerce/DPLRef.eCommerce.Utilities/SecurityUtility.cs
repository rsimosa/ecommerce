using System;

namespace DPLRef.eCommerce.Utilities
{
    class SecurityUtility : UtilityBase, ISecurityUtility
    {
        public bool SellerAuthenticated()
        {
            //authenticate so long as the token <> "Invalid", NULL or ""
            if (Context.AuthToken == "Invalid" || String.IsNullOrEmpty(Context.AuthToken))
            {
                return false;
            }
            return true;
        }

        public bool BackOfficeAdminAuthenticated()
        {
            //authenticate so long as the token <> "Invalid", NULL or ""
            if (Context.AuthToken == "Invalid" || String.IsNullOrEmpty(Context.AuthToken))
            {
                return false;
            }
            return true;
        }
    }
}
