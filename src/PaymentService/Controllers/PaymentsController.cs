using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Data;
using PaymentService.UseCases.Commands;
using PaymentService.UseCases.Queries;
namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _context;
        public PaymentsController(IMediator mediator, PaymentDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            var validator = new CreatePaymentCommandValidator();
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }
        [HttpPut("updateStatus/{paymentId}/{status}")]
        public async Task<IActionResult> UpdatePaymentStatus(long paymentId, bool status)
        {
            var command = new UpdatePaymentStatusCommand
            {
                PaymentId = paymentId,
                Status = status
            };

            var validator = new UpdatePaymentStatusCommandValidator(_context);
            var validationResult = await validator.ValidateAsync(command); 
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command);
            if(!result) return NotFound(new { Message = $"Платеж с Id {paymentId} не найден" });
            return Ok("Статус успешно обновлен");
        }

        [HttpGet("get/{paymentId}")]
        public async Task<IActionResult> GetPayment(long paymentId)
        {
            var query = new GetPaymentQuery { PaymentId = paymentId };

            var validator = new GetPaymentQueryValidator(_context);
            var validationResult = await validator.ValidateAsync(query); 
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);


            var payment = await _mediator.Send(query);
            if (payment == null) return NotFound(new { Message = $"Платеж с Id {paymentId} не найден" });
            return Ok(payment);
        }
        
    }
}
