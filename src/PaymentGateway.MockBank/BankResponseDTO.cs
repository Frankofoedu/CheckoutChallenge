using PaymentGateway.Shared;

namespace PaymentGateway.MockBank
{
    public class BankResponseDTO
    {
        public Guid BankTransactionId { get; set; }
        public Guid PaymentTransactionId { get; }
        public TransactionStatus Status { get; }
        public string Reason { get; }

        public BankResponseDTO(
            Guid paymentTransactionId,
            Guid bankTransactionId,
            TransactionStatus status,
            string reason)
        {
            PaymentTransactionId = paymentTransactionId;
            BankTransactionId = bankTransactionId;
            Status = status;
            Reason = reason;
        }
    }
}