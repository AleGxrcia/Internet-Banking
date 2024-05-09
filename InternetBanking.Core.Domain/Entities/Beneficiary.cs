using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiary :  AuditableBaseEntity
    {
        public string UserOwnerId { get; set; }
        public string AccountNumberBeneficiary { get; set; }
    }
}
