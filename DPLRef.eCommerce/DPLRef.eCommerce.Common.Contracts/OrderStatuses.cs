namespace DPLRef.eCommerce.Common.Contracts
{
    public enum OrderStatuses
    {
        Created = 0,
        Authorized = 10,
        Captured = 15,
        Shipped = 20,
        Completed = 30,
        Failed = 100
    }
}
