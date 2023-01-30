using PaymentGateway.Shared;

namespace PaymentGateway.MockBank
{
    public interface IBankService
    {
        /// <summary>
        /// Process a transaction through a bank
        /// </summary>
        /// <param name="request">the transaction request</param>
        /// <returns>A transaction response from the bank</returns>
        Task<BankResponseDTO> ProcessTransactionAsync(BankRequestDTO request);
    }

    public class MockBankService : IBankService
    {
        private readonly Random _Random = new();

        public async Task<BankResponseDTO> ProcessTransactionAsync(BankRequestDTO request)
        {
            // Set Random approval status
            var transactionStatuses = Enum.GetValues(typeof(TransactionStatus));
            var randomTransactionStatus = (TransactionStatus)transactionStatuses.GetValue(_Random.Next(0, transactionStatuses.Length))!;

            //get reason from status enum description
            var reason = randomTransactionStatus.GetDescription();

            var bankTransactionId = Guid.NewGuid();
            return new BankResponseDTO(request.PaymentId, bankTransactionId, randomTransactionStatus, reason);
        }
    }
}