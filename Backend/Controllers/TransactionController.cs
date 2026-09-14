using Backend.DTOs.Transaction;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(
            ITransactionService service)
        {
            _service = service;
        }


        // POST: api/transactions
        // Customer creates payment attempt
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] TransactionRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }


            var result =
                await _service.CreateTransactionAsync(
                    request,
                    userId.Value);


            if (result == null)
            {
                return BadRequest(new
                {
                    message =
                        "Unable to create transaction. Please check your order and payment method."
                });
            }


            return Ok(result);
        }


        // GET: api/transactions
        // Customer gets their own transactions
        [HttpGet]
        public async Task<IActionResult>
            GetMyTransactions()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }


            var transactions =
                await _service.GetMyTransactionsAsync(
                    userId.Value);


            return Ok(transactions);
        }


        // GET: api/transactions/{id}
        // Customer gets their own transaction
        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetById(int id)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }


            var transaction =
                await _service.GetByIdAsync(
                    id,
                    userId.Value);


            if (transaction == null)
            {
                return NotFound(new
                {
                    message =
                        "Transaction not found."
                });
            }


            return Ok(transaction);
        }

        // ADMIN ROUTES

        [HttpGet("admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transaction = await _service.GetAllTransactionsAsync();

            if (transaction == null)
            {
                return NotFound("Transaction is null");
            }

            return Ok(transaction);
        }


        [HttpGet("admin/{transactionId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var result =
                await _service.AdminGetByIdAsync(transactionId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/transactions/admin/{id}/payment
        // Admin updates any user's payment
        [HttpPut("admin/{id}/payment")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AdminUpdatePayment(
            int id,
            [FromBody] TransactionUpdateRequest request)
        {
            var result =
                await _service.AdminUpdatePaymentAsync(
                    id,
                    request);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Unable to update payment. The status is already updated!"
                });
            }

            return Ok(new
            {
                message = "Payment status updated successfully."
            });
        }

        // Get current user ID from JWT
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);


            if (userIdClaim == null)
            {
                return null;
            }


            if (!int.TryParse(
                userIdClaim.Value,
                out int userId))
            {
                return null;
            }


            return userId;
        }
    }
}