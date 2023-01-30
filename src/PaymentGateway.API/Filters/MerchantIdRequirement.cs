using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace PaymentGateway.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    internal class MerchantIdRequirementAttribute : TypeFilterAttribute
    {
        public MerchantIdRequirementAttribute() : base(typeof(MerchantIdRequirementFilter))
        {
        }
    }

    internal class MerchantIdRequirementFilter : IAsyncActionFilter
    {
        public IConfiguration _configuration;
        public ILogger<MerchantIdRequirementAttribute> _logger;

        public MerchantIdRequirementFilter(IConfiguration configuration, ILogger<MerchantIdRequirementAttribute> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var merchantIdHeaderValue = context.HttpContext.Request.Headers["MerchantId"].FirstOrDefault();
            if (merchantIdHeaderValue is null)
            {
                context.Result = new UnauthorizedObjectResult("No merchant Id provided");
                return;
            }

            //get static merchant ids from config file
            var merchantIds = _configuration.GetSection("MerchantKeys")?.GetChildren()?.Select(x => x.Get<string>()).ToList();

            //validations
            if (merchantIds is null || merchantIds.Count < 1)
            {
                _logger.LogError("Merchant keys not setup");
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            if (!merchantIds.Contains(merchantIdHeaderValue))
            {
                context.Result = new UnauthorizedObjectResult("Provide valid merchant Id");
                return;
            }

            //attach merchantId to claims
            var claims = new List<Claim>
            {
                new Claim("MerchantId", merchantIdHeaderValue)
            };
            var appIdentity = new ClaimsIdentity(claims);
            context.HttpContext.User.AddIdentity(appIdentity);

            await next();
        }
    }
}