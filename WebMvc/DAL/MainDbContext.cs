using Microsoft.EntityFrameworkCore;
using WebMvc.DAL.Models;

namespace WebMvc.DAL;

public class MainDbContext: DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderPosition> OrderPositions { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
    public async Task<Order> GetOrderById(int orderId)
    {
        return await Orders.FindAsync(orderId);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>()
            .HasKey(x => new { x.WarehouseId, x.ItemId });

        modelBuilder.Entity<Order>()
            .Property(x => x.Status)
            .HasConversion<int>();

        modelBuilder.Entity<OrderPosition>()
            .HasKey(x => new { x.WarehouseId, x.ItemId, x.OrderId });

    }
}
