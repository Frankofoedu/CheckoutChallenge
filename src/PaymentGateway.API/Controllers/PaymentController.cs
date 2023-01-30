using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Filters;
using PaymentGateway.API.Idempotency;
using PaymentGateway.Core.Services;
using PaymentGateway.Core.ViewModels;
using System.Security.Claims;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentGatewayService paymentGatewayService)
        {
            _logger = logger;
            _paymentGatewayService = paymentGatewayService;
        }

        /// <summary>
        /// Process a merchant's transaction
        /// </summary>
        /// <param name="request">The transaction body</param>
        /// <returns></returns>
        [HttpPost()]
        [MerchantIdRequirement()]
        [ServiceFilter(typeof(IdempotencyFilter))]
        [ProducesResponseType(typeof(CreatePaymentResponseViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ProcessPayment(CreatePaymentRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            try
            {
                request.MerchantId = Guid.Parse(User.FindFirstValue("MerchantId"));

                var result = await _paymentGatewayService.ProcessPayment(request);
                if (result.HasError)
                {
                    return BadRequest(result.ErrorMessageString);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error :", ex);
                return Problem("An error occured while processing your request");
            }
        }

        /// <summary>
        /// Request details of a previous payment
        /// </summary>
        /// <param name="paymentId">the Id of the payment or transaction</param>
        /// <returns>A payment</returns>
        [HttpGet("{transactionId}")]
        [MerchantIdRequirement()]
        [ProducesResponseType(200, Type = typeof(GetPaymentResponseViewModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPayment([FromRoute] Guid transactionId)
        {
            try
            {
                var request = new GetPaymentQueryViewModel
                {
                    MerchantId = Guid.Parse(User.FindFirstValue("MerchantId")),
                    TransactionId = transactionId
                };
                var result = await _paymentGatewayService.GetPaymentRecord(request);
                if (result.HasError)
                {
                    return BadRequest(result.ErrorMessageString);
                }

                if (result.Data is null)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error :", ex);
                return Problem("An error occured while processing your request");
            }
        }
    }
}