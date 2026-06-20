using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.UseCases.Commands;
using PaymentService.UseCases.Queries;
namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(new { PaymentId = paymentId });
        }
        [HttpPut("updateStatus/{paymentId}/{status}")]
        public async Task<IActionResult> UpdatePaymentStatus(long paymentId, bool status)
        {
            var command = new UpdatePaymentStatusCommand
            {
                PaymentId = paymentId,
                Status = status
            };

            var result = await _mediator.Send(command);
            if(!result) return NotFound(new { Message = $"Платеж с Id {paymentId} не найден" });
            return Ok(new { Message = $"Статус платежа с Id {paymentId} успешно обновлен" });
        }

        [HttpGet("get/{paymentId}")]
        public async Task<IActionResult> GetPayment(long paymentId)
        {
            var query = new GetPaymentQuery { PaymentId = paymentId };
            var payment = await _mediator.Send(query);
            if (payment == null) return NotFound(new { Message = $"Платеж с Id {paymentId} не найден" });
            return Ok(payment);
        }
        
    }
}
