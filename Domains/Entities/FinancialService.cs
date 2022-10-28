namespace Domains.Entities
{
    public class FinancialServiceInfo
    {
        public string? FinancialService { get; set; }
        public BankFinancialServiceInfo? BankFinancialServiceInfo { get; set; }
        public MobileFinancialServiceInfo? MobileFinancialServiceInfo { get; set; }
    }

    public class BankFinancialServiceInfo
    {
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountHolderName { get; set; }
        public long AccountNumber { get; set; }
        public long RoutingNumber { get; set; }
    }

    public class MobileFinancialServiceInfo
    {
        public int PhoneNumber { get; set; }
        public string? ServiceProvider { get; set; }
    }
}
