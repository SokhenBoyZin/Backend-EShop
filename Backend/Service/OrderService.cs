using Backend.Db;
using Backend.DTOs.DeliveryLocation;
using Backend.DTOs.Order;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }


        // CREATE ORDER
        public async Task<OrderResponse?> CreateOrderAsync(
            OrderRequest request,
            int userId)
        {
            if (request == null)
            {
                return null;
            }

            if (request.Items == null ||
                request.Items.Count == 0)
            {
                return null;
            }

            foreach (var item in request.Items)
            {
                if (item.ProductVariantId <= 0 ||
                    item.Quantity <= 0)
                {
                    return null;
                }
            }


            // Check delivery location belongs to current user
            var deliveryLocation =
                await _context.DeliveryLocations
                    .FirstOrDefaultAsync(x =>
                        x.DeliveryLocationId ==
                            request.DeliveryLocationId &&
                        x.UserId == userId);

            if (deliveryLocation == null)
            {
                return null;
            }


            // Create order
            var order = new Order
            {
                UserId = userId,

                DeliveryLocationId =
                    deliveryLocation.DeliveryLocationId,

                DeliveryMethod =
                    request.DeliveryMethod,

                OrderStatus =
                    OrderStatus.PENDING,

                CreatedAt =
                    DateTime.UtcNow
            };


            decimal subtotal = 0;


            foreach (var item in request.Items)
            {
                var variant =
                    await _context.ProductVariants
                        .Include(x => x.Product)
                        .Include(x => x.Color)
                        .Include(x => x.Capacity)
                        .Include(x => x.ConnectivityType)
                        .FirstOrDefaultAsync(x =>
                            x.ProductVariantId ==
                            item.ProductVariantId);

                // Variant does not exist
                if (variant == null)
                {
                    return null;
                }

                // Product archived
                if (variant.Product.IsArchived)
                {
                    return null;
                }

                // Not enough stock
                if (variant.StockQuantity <
                    item.Quantity)
                {
                    return null;
                }


                // Get price from database
                decimal unitPrice =
                    variant.Price;


                // Calculate item total
                decimal itemTotal =
                    unitPrice * item.Quantity;

                subtotal += itemTotal;


                // Reserve stock
                variant.StockQuantity -=
                    item.Quantity;


                // Create OrderItem
                var orderItem = new OrderItem
                {
                    ProductVariantId =
                        variant.ProductVariantId,

                    Quantity =
                        item.Quantity,

                    UnitPrice =
                        unitPrice
                };

                order.OrderItems.Add(orderItem);
            }


            // Calculate delivery fee
            decimal deliveryFee =
                CalculateDeliveryFee(
                    request.DeliveryMethod);


            // Calculate total
            decimal totalAmount =
                subtotal + deliveryFee;


            order.Subtotal =
                subtotal;

            order.DeliveryFee =
                deliveryFee;

            order.TotalAmount =
                totalAmount;


            // Save order
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();


            // Return response
            return await GetByIdAsync(
                order.OrderId,
                userId);
        }


        // GET MY ORDERS
        public async Task<List<OrderResponse>>
            GetMyOrdersAsync(int userId)
        {
            var orders =
                await _context.Orders

                    .Where(x =>
                        x.UserId == userId)

                    .Include(x =>
                        x.DeliveryLocation)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Product)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Color)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Capacity)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.ConnectivityType)

                    .OrderByDescending(x =>
                        x.CreatedAt)

                    .ToListAsync();


            return orders
                .Select(MapToResponse)
                .ToList();
        }


        // GET ORDER BY ID
        public async Task<OrderResponse?>
            GetByIdAsync(
                int orderId,
                int userId)
        {
            var order =
                await _context.Orders

                    .Where(x =>
                        x.OrderId == orderId &&
                        x.UserId == userId)

                    .Include(x =>
                        x.DeliveryLocation)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Product)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Color)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.Capacity)

                    .Include(x =>
                        x.OrderItems)
                        .ThenInclude(x =>
                            x.ProductVariant)
                        .ThenInclude(x =>
                            x.ConnectivityType)

                    .FirstOrDefaultAsync();


            if (order == null)
            {
                return null;
            }


            return MapToResponse(order);
        }


        // CANCEL ORDER
        public async Task<bool>
            CancelOrderAsync(
                int orderId,
                int userId)
        {
            var order =
                await _context.Orders

                    .Include(x =>
                        x.OrderItems)

                    .FirstOrDefaultAsync(x =>
                        x.OrderId == orderId &&
                        x.UserId == userId);


            if (order == null)
            {
                return false;
            }


            // Only PENDING order can be cancelled
            if (order.OrderStatus !=
                OrderStatus.PENDING)
            {
                return false;
            }


            // Return stock
            foreach (var item in order.OrderItems)
            {
                var variant =
                    await _context.ProductVariants
                        .FirstOrDefaultAsync(x =>
                            x.ProductVariantId ==
                            item.ProductVariantId);

                if (variant != null)
                {
                    variant.StockQuantity +=
                        item.Quantity;
                }
            }


            order.OrderStatus =
                OrderStatus.CANCELLED;


            await _context.SaveChangesAsync();

            return true;
        }


        // UPDATE ORDER STATUS
        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order =
                await _context.Orders
                    .FirstOrDefaultAsync(x =>
                        x.OrderId == orderId);


            if (order == null)
            {
                return false;
            }


            // Only allow valid status transitions
            bool validTransition =
                order.OrderStatus switch
                {
                    OrderStatus.PENDING =>
                        newStatus ==
                        OrderStatus.CANCELLED,

                    OrderStatus.PROCESSING =>
                        newStatus ==
                        OrderStatus.SHIPPING,

                    OrderStatus.SHIPPING =>
                        newStatus ==
                        OrderStatus.DELIVERED,

                    _ => false
                };


            if (!validTransition)
            {
                return false;
            }


            order.OrderStatus =
                newStatus;


            await _context.SaveChangesAsync();

            return true;
        }


        // DELIVERY FEE
        private decimal CalculateDeliveryFee(
            DeliveryMethod deliveryMethod)
        {
            return deliveryMethod switch
            {
                DeliveryMethod.PICKUP => 0m,

                DeliveryMethod.MOTOR => 5.00m,

                _ => 0m
            };
        }


        // GET ALL ORDERS
        // ADMIN ONLY
        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(x => x.DeliveryLocation)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Color)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Capacity)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.ConnectivityType)
                .OrderByDescending(x => x.OrderId)
                .ToListAsync();

            return orders
                .Select(MapToResponse)
                .ToList();
        }

        // GET ANY ORDER BY ID
        // ADMIN ONLY
        public async Task<OrderResponse?> AdminGetByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(x => x.DeliveryLocation)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Color)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Capacity)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.ConnectivityType)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            return MapToResponse(order);
        }



        // MAP ORDER TO RESPONSE
        private OrderResponse
            MapToResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId =
                    order.OrderId,

                UserId =
                    order.UserId,

                DeliveryLocation =
                    new DeliveryLocationResponse
                    {
                        DeliveryLocationId =
                            order.DeliveryLocation
                                .DeliveryLocationId,

                        ContactName =
                            order.DeliveryLocation
                                .ContactName,

                        PhoneNumber =
                            order.DeliveryLocation
                                .PhoneNumber,

                        City =
                            order.DeliveryLocation
                                .City,

                        District =
                            order.DeliveryLocation
                                .District,

                        Address =
                            order.DeliveryLocation
                                .Address
                    },

                Subtotal =
                    order.Subtotal,

                DeliveryMethod =
                    order.DeliveryMethod,

                DeliveryFee =
                    order.DeliveryFee,

                TotalAmount =
                    order.TotalAmount,

                OrderStatus =
                    order.OrderStatus,

                CreatedAt =
                    order.CreatedAt,

                OrderItems =
                    order.OrderItems
                        .Select(item =>
                            new OrderItemResponse
                            {
                                OrderItemId =
                                    item.OrderItemId,

                                ProductVariantId =
                                    item.ProductVariantId,

                                Quantity =
                                    item.Quantity,

                                UnitPrice =
                                    item.UnitPrice,

                                ProductName =
                                    item.ProductVariant
                                        .Product.Name,

                                Color =
                                    item.ProductVariant
                                        .Color.Name,

                                Capacity =
                                    item.ProductVariant
                                        .Capacity.SizeLabel,

                                Connectivity =
                                    item.ProductVariant
                                        .ConnectivityType.Name
                            })
                        .ToList()
            };
        }
    }
}