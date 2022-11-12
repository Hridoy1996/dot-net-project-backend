using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Entities
{
    [BsonIgnoreExtraElements]
    public class FinancialServiceInfo
    {
        public string? Type { get; set; }
        public BankFinancialServiceInfo? BankFinancialServiceInfo { get; set; }
        public MobileFinancialServiceInfo? MobileFinancialServiceInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class BankFinancialServiceInfo
    {
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountHolderName { get; set; }
        public long AccountNumber { get; set; }
        public long RoutingNumber { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class MobileFinancialServiceInfo
    {
        public int PhoneNumber { get; set; }
        public string? ServiceProvider { get; set; }
    }
}
