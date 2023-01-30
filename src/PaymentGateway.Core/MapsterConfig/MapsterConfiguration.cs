using Mapster;
using PaymentGateway.Core.Entities;
using PaymentGateway.Core.ViewModels;

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
        }
    }
}