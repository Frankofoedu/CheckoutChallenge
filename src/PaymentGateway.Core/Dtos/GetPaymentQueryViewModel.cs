namespace PaymentGateway.Core.ViewModels
{
    public class GetPaymentQueryViewModel
    {
        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
    }
}