namespace PaymentGateway.API.Idempotency
{
    public class IdempotencyConfiguration
    {
        public string HeaderName { get; set; }
        public TimeSpan TimeToLiveMaster { get; set; }
        public TimeSpan TimeToLiveDeprecation { get; set; }
    }

    public static class IdempotencyUtil
    {
        public static IServiceCollection RegisterIdempotentConfig(this IServiceCollection services)
        {
            services.AddTransient<IdempotencyFilter>();
            services.AddSingleton<IIdempotentStorage, InMemoryStorage>();
            services.Configure<IdempotencyConfiguration>(config =>
            {
                // Defines the header from where the idempotency-key is read.
                config.HeaderName = "x-idem-key";
                // Defines how much time a cache response is valid.
                config.TimeToLiveDeprecation = TimeSpan.FromSeconds(10);
                // We need to specify a time-to-live in case the api fails to process a request.
                config.TimeToLiveMaster = TimeSpan.FromSeconds(15);
            });

            return services;
        }
    }
}