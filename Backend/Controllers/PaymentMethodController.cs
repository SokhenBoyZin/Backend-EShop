using Backend.DTOs.PaymentMethod;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/payment-methods")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _service;

        public PaymentMethodController(
            IPaymentMethodService service)
        {
            _service = service;
        }


        // GET: api/payment-methods
        // Customer + Admin
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var paymentMethods = await _service.GetAllAsync();

            return Ok(paymentMethods);
        }


        // GET: api/payment-methods/{id}
        // Customer + Admin
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var paymentMethod = await _service.GetByIdAsync(id);

            if (paymentMethod == null)
            {
                return NotFound(new
                {
                    message = "Payment method not found."
                });
            }

            return Ok(paymentMethod);
        }


        // POST: api/payment-methods
        // Admin only
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(
            [FromBody] PaymentMethodRequest request)
        {
            var paymentMethod =
                await _service.CreateAsync(request);

            if (paymentMethod == null)
            {
                return NotFound(new
                {
                    message = "Payment method not found."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = paymentMethod.PaymentMethodId },
                paymentMethod);
        }


        // PUT: api/payment-methods/{id}
        // Admin only
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] PaymentMethodRequest request)
        {
            var paymentMethod =
                await _service.UpdateAsync(id, request);

            if (paymentMethod == null)
            {
                return NotFound(new
                {
                    message = "Payment method not found."
                });
            }

            return Ok(paymentMethod);
        }


        // DELETE: api/payment-methods/{id}
        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Payment method not found."
                });
            }

            return Ok(new
            {
                message = "Payment method deleted successfully."
            });
        }
    }
}
