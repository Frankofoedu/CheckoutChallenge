using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.Core.DataAccess;
using PaymentGateway.Core.Entities;
using PaymentGateway.Core.ViewModels;
using PaymentGateway.MockBank;
using PaymentGateway.Shared;

namespace PaymentGateway.Core.Services
{
    public interface IPaymentGatewayService
    {
        Task<ResultModel<GetPaymentResponseViewModel>> GetPaymentRecord(GetPaymentQueryViewModel transactionId);

        Task<ResultModel<CreatePaymentResponseViewModel>> ProcessPayment(CreatePaymentRequestDto request);
    }

    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IBankService _bankService;
        private IAsyncRepository<Transaction> _transactionRepository;
        private readonly ILogger<PaymentGatewayService> _Logger;

        public PaymentGatewayService(IBankService bankService, IAsyncRepository<Transaction> transactionRepository, ILogger<PaymentGatewayService> logger)
        {
            _bankService = bankService;
            _transactionRepository = transactionRepository;
            _Logger = logger;
        }

        public async Task<ResultModel<GetPaymentResponseViewModel>> GetPaymentRecord(GetPaymentQueryViewModel request)
        {
            var transaction = await _transactionRepository.GetAll()
                .Where(t => t.TransactionId == request.TransactionId && t.MerchantId == request.MerchantId)
                .FirstOrDefaultAsync();

            _Logger.LogInformation($"Payment requested with id {request.TransactionId}");

            if (transaction is null)
            {
                return new ResultModel<GetPaymentResponseViewModel>(data: null, message: $"No transaction exists for transaction Id : {request.TransactionId}");
            }

            var transactionResponse = transaction.Adapt<GetPaymentResponseViewModel>();

            return new ResultModel<GetPaymentResponseViewModel>(transactionResponse);
        }

        public async Task<ResultModel<CreatePaymentResponseViewModel>> ProcessPayment(CreatePaymentRequestDto request)
        {
            //save transaction before calling external bank
            var transaction = request.Adapt<Transaction>();
            await _transactionRepository.AddAsync(transaction);

            //call external bank
            var result = await _bankService.ProcessTransactionAsync(new BankRequestDTO
            {
                CardCvv = request.Card.Cvv,
                CardExpiryMonth = request.Card.ExpiryMonth,
                CardExpiryYear = request.Card.ExpiryYear,
                CardHolderName = request.Card.OwnerName,
                CardNumber = request.Card.Number,
                Currency = request.Currency,
                PaymentId = transaction.TransactionId,
                TransactionAmount = request.Amount
            });

            //update transaction
            //use hangfire to process this
            transaction.DateUpdated = DateTime.UtcNow;
            transaction.BankTransactionId = result.BankTransactionId;
            transaction.Reason = result.Reason;
            transaction.TransactionStatus = result.Status;

            await _transactionRepository.UpdateAsync(transaction);

            var response = new CreatePaymentResponseViewModel
            {
                TransactionId = transaction.TransactionId,
                BankTransactionId = transaction.BankTransactionId,
                DateCreated = transaction.DateCreated,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Description = transaction.Description,
                Card = new CardDetails
                {
                    Cvv = transaction.CardCvv,
                    ExpiryMonth = transaction.CardExpiryMonth,
                    ExpiryYear = transaction.CardExpiryYear,
                    Number = transaction.CardNumber,
                    OwnerName = transaction.CardHolderName
                }
            };
            return new ResultModel<CreatePaymentResponseViewModel>(response);
        }
    }
}