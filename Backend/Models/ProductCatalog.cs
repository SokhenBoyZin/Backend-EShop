using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsArchived { get; set; } = false;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty ;
        // Specs
        public string ChipName { get; set; } = string.Empty; // "Apple M4 chip"
        public string CpuCores { get; set; } = string.Empty; // "8-core CPU"
        public string GpuCores { get; set; } = string.Empty; // "9-core GPU"
        public int RamGb { get; set; } // 12Gb
        public string DisplayName { get; set; } = string.Empty; // "Liquid Retina"
        public string DisplayResolution { get; set; } = string.Empty; // "2732-by-2048"
        public int MainCameraMp { get; set; }
        public int FrontCameraMp { get; set; }
        public string OsVersion { get; set; } = string.Empty; // "iPadOS 26"
        public bool IsArchived { get; set; } // soft delete 

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        // Navigation Property
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class Color
    {
        public int ColorId { get; set; }
        public string Name { get; set; } = string.Empty; // "Space Gray", "Blue", "Starlight", "Purple"
        // one to many
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class Capacity
    {
        public int CapacityId { get; set; }
        public string SizeLabel { get; set; } = string.Empty; // "128GB", "256GB", etc.
        // one to many
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class ConnectivityType
    {
        public int ConnectivityTypeId { get; set; }
        public string Name { get; set; } = string.Empty; // "Wifi", "Wifi + Cellular"
        // one to many
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

    public class ProductVariant
    {
        public int ProductVariantId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } // Product price each capacity and connectivity Eg: 1099, 1199, 1299... 
        public int StockQuantity { get; set; }
        //ForeignKey Product
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        //ForeignKey Color
        public int ColorId { get; set; }
        [ForeignKey(nameof(ColorId))]
        public Color Color { get; set; } = null!;
        //ForeignKey Capacity
        public int CapacityId { get; set; }
        [ForeignKey(nameof(CapacityId))]
        public Capacity Capacity { get; set; } = null!;
        //ForeignKey Connectivity
        public int ConnectivityTypeId { get; set; }
        [ForeignKey(nameof(ConnectivityTypeId))]
        public ConnectivityType ConnectivityType { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class DeliveryLocation
    {
        public int DeliveryLocationId { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public string ContactName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City {  get; set; } = string.Empty;   
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }

    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }


    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }
        public int ProductVariantId { get; set; }
        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant ProductVariant { get; set; } = null!;

    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
        public int PaymentMethodId { get; set; }
        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionRef { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public int DeliveryLocationId { get; set; }
        [ForeignKey(nameof(DeliveryLocationId))]
        public DeliveryLocation DeliveryLocation { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DeliveryFee { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}   