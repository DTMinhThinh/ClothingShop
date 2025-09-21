using ClothingStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Data
{
	public class ClothingStoreDBContext(DbContextOptions<ClothingStoreDBContext> options) : DbContext(options)
	{
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<StockReceipt> StockReceipts { get; set; }
		public DbSet<StockReceiptItem> StockReceiptItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }

		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Cấu hình TPT cho inheritance
			modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);
			modelBuilder.Entity<Customer>().ToTable("Customers").HasBaseType<User>();
			modelBuilder.Entity<Employee>().ToTable("Employees").HasBaseType<User>();

			// 1. Category - Product (1-N)
			modelBuilder.Entity<Category>()
				.HasMany<Product>() // Không cần navigation property
				.WithOne() // Không cần navigation property
				.HasForeignKey(p => p.CategoryID)
				.OnDelete(DeleteBehavior.Restrict);

			// 2. Customer - Order (1-N)
			modelBuilder.Entity<Customer>()
				.HasMany<Order>() // Không cần Orders collection
				.WithOne() // Không cần Customer navigation
				.HasForeignKey(o => o.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			// 3. Employee - Order (1-N) - Employee phê duyệt Orders
			modelBuilder.Entity<Employee>()
				.HasMany<Order>() // Không cần ApprovedOrders collection
				.WithOne() // Không cần Employee navigation
				.HasForeignKey(o => o.EmployeeId)
				.OnDelete(DeleteBehavior.SetNull);

			// 4. Order - OrderItem (1-N)
			modelBuilder.Entity<Order>()
				.HasMany<OrderItem>() // Không cần OrderItems collection
				.WithOne() // Không cần Order navigation
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			// 5. Product - OrderItem (1-N)
			modelBuilder.Entity<Product>()
				.HasMany<OrderItem>() // Không cần OrderItems collection
				.WithOne() // Không cần Product navigation
				.HasForeignKey(oi => oi.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			// 6. StockReceipt - StockReceiptItem (1-N)
			modelBuilder.Entity<StockReceipt>()
				.HasMany<StockReceiptItem>() // Không cần StockReceiptItems collection
				.WithOne() // Không cần StockReceipt navigation
				.HasForeignKey(sri => sri.ReceiptId)
				.OnDelete(DeleteBehavior.Cascade);

			// 7. Product - StockReceiptItem (1-N)
			modelBuilder.Entity<Product>()
				.HasMany<StockReceiptItem>() // Không cần ReceiptItems collection
				.WithOne() // Không cần Product navigation
				.HasForeignKey(sri => sri.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			// 8. Employee - Role (N-1)
			modelBuilder.Entity<Employee>()
				.HasOne<Role>() // Không cần Role navigation
				.WithMany() // Không cần Employees collection
				.HasForeignKey(e => e.RoleID)
				.OnDelete(DeleteBehavior.SetNull);

			// 9. StockReceipt - Employee (N-1) - Employee tạo StockReceipt
			modelBuilder.Entity<StockReceipt>()
				.HasOne<Employee>() // Không cần Employee navigation
				.WithMany() // Không cần navigation property
				.HasForeignKey(sr => sr.EmployeeId)
				.OnDelete(DeleteBehavior.Restrict);

			// Cấu hình khóa
			modelBuilder.Entity<OrderItem>()
				.HasKey(oi => new { oi.OrderId, oi.ProductId });

			modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
			modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
			modelBuilder.Entity<StockReceiptItem>().HasKey(sri => sri.ReceiptItemId);

		}
	}
}

