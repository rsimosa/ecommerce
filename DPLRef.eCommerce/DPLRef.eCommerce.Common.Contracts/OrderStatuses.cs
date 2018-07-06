namespace DPLRef.eCommerce.Common.Contracts
{
    // Because of issue casting int to int64 with Sqlite we made
    // this enum a long instead of an int.
    public enum OrderStatuses : long
    {
        Created = 0,
        Authorized = 10,
        Captured = 15,
        Shipped = 20,
        Completed = 30,
        Failed = 100
    }
}
