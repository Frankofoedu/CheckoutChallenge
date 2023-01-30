using System.ComponentModel;

namespace PaymentGateway.Shared
{
    public enum TransactionStatus
    {
        [Description("The transaction was successful")]
        Approved = 1,

        [Description("The request was declined. Please retry again.")]
        Declined = 2,

        [Description("The request failed. Card number provided is not valid.")]
        InvalidCardNumber = 3,

        [Description("The request was declined. Insufficient funds.")]
        InsufficientFunds = 4,

        [Description("The request was declined. Card is restricted. Please contact your bank.")]
        RestrictedCard = 5,
    }
}