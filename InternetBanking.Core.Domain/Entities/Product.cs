using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
        public double Balance { get; set; }
        public double Debt { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
