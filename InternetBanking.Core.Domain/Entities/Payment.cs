using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
    public class Payment : AuditableBaseEntity
    {
        public string SourceAccountNumber { get; set; }
        public string DestinationAccountNumber { get; set; }
        public double Amount { get; set; }
        public int PaymentTypeId { get; set; }
    }
}
