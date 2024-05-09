namespace InternetBanking.Core.Application.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        public int TotalProductsCreated { get; set; }
        public int TotalPaymentsMade { get; set; }
        public int PaymentsMadeLast24Hours { get; set; }
        public int TotalTransactionsMade { get; set; }
        public int TransactionsMadeLast24Hours { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalInactiveUsers { get; set; }

    }
}
