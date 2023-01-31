using Mapster;
using PaymentGateway.Core.Entities;
using PaymentGateway.Core.ViewModels;
using PaymentGateway.MockBank;

namespace PaymentGateway.Core.MapsterConfig
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreatePaymentRequestDto, Transaction>()
                .Map(dest => dest.CardCvv, src => src.Card.Cvv)
                .Map(dest => dest.CardExpiryMonth, src => src.Card.ExpiryMonth)
                .Map(dest => dest.CardExpiryYear, src => src.Card.ExpiryYear)
                .Map(dest => dest.CardHolderName, src => src.Card.OwnerName)
                .Map(dest => dest.CardNumber, src => src.Card.Number);

            config.NewConfig<Transaction, GetPaymentResponseDto>()
               .Map(dest => dest.Card.Cvv, src => src.CardCvv)
               .Map(dest => dest.Card.ExpiryMonth, src => src.CardExpiryMonth)
               .Map(dest => dest.Card.ExpiryYear, src => src.CardExpiryYear)
               .Map(dest => dest.Card.OwnerName, src => src.CardHolderName)
               .Map(dest => dest.Card.Number, src => src.CardNumber);

            config.NewConfig<Transaction, CreatePaymentResponseDto>()
               .Map(dest => dest.Card.Cvv, src => src.CardCvv)
               .Map(dest => dest.Card.ExpiryMonth, src => src.CardExpiryMonth)
               .Map(dest => dest.Card.ExpiryYear, src => src.CardExpiryYear)
               .Map(dest => dest.Card.OwnerName, src => src.CardHolderName)
               .Map(dest => dest.Card.Number, src => src.CardNumber);

            config.NewConfig<CreatePaymentRequestDto, BankRequestDTO>()
             .Map(dest => dest.CardCvv, src => src.Card.Cvv)
             .Map(dest => dest.CardExpiryMonth, src => src.Card.ExpiryMonth)
             .Map(dest => dest.CardExpiryYear, src => src.Card.ExpiryYear)
             .Map(dest => dest.CardHolderName, src => src.Card.OwnerName)
             .Map(dest => dest.CardNumber, src => src.Card.Number);
        }
    }
}

//TODO: Add more mapping configs