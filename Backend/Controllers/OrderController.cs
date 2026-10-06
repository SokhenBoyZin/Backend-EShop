using Backend.DTOs.Order;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }


        // POST: api/orders
        // Customer creates an order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] OrderRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _service.CreateOrderAsync(
                request,
                userId.Value);

            if (result == null)
            {
                return BadRequest(new
                {
                    message =
                        "Unable to create order. Please check your items, stock, and delivery location."
                });
            }

            return Ok(result);
        }


        // GET: api/orders
        // Customer gets their own orders
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var orders = await _service.GetMyOrdersAsync(
                userId.Value);

            return Ok(orders);
        }


        // GET: api/orders/{id}
        // Customer gets their own order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var order = await _service.GetByIdAsync(
                id,
                userId.Value);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(order);
        }


        // POST: api/orders/{id}/cancel
        // Customer cancels their own order
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _service.CancelOrderAsync(
                id,
                userId.Value);

            if (!result)
            {
                return BadRequest(new
                {
                    message =
                        "Order cannot be cancelled."
                });
            }

            return Ok(new
            {
                message =
                    "Order cancelled successfully."
            });
        }


        // PUT: api/orders/{id}/status
        // Admin updates shipping/order status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateOrderStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request is required."
                });
            }

            var result = await _service.UpdateStatusAsync(
                id,
                request.Status);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Invalid order status update."
                });
            }

            return Ok(new
            {
                message =
                    "Order status updated successfully."
            });
        }

        // GET: api/orders/admin
        // Admin gets ALL users' orders
        [HttpGet("admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders =
                await _service.GetAllOrdersAsync();

            return Ok(orders);
        }


        // GET: api/orders/admin/{id}
        // Admin gets ANY user's order
        [HttpGet("admin/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AdminGetById(int id)
        {
            var order =
                await _service.AdminGetByIdAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(order);
        }

        // GET: api/orders/recent-all
        // Admin/Staff gets recent orders across ALL users
        [HttpGet("recent-all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllRecentOrders([FromQuery] int count = 4)
        {
            var recentOrders = await _service.GetAllRecentOrdersAsync(count);

            return Ok(recentOrders);
        }

        // GET: api/orders/admin/pending
        // Admin gets ALL pending orders across all users
        [HttpGet("admin/pending")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAdminPendingOrders()
        {
            var pendingOrders = await _service.GetAdminPendingOrdersAsync();

            return Ok(pendingOrders);
        }

        // Get current user ID from JWT
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

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