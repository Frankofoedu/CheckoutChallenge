namespace PaymentGateway.Core.ViewModels
{
    public class GetPaymentQueryDto
    {
        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
    }
}