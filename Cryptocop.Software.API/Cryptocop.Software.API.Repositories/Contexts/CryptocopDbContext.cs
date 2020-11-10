using Cryptocop.Software.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Contexts
{
  public class CryptocopDbContext : DbContext
  {
    public CryptocopDbContext(DbContextOptions<CryptocopDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Address>()
          .HasOne(a => a.User)
          .WithMany(u => u.Addresses);

      modelBuilder.Entity<PaymentCard>()
          .HasOne(p => p.User)
          .WithMany(u => u.PaymentCards);

      modelBuilder.Entity<ShoppingCart>()
          .HasOne(s => s.User)
          .WithOne(u => u.ShopppingCart);

      modelBuilder.Entity<ShoppingCartItem>()
          .HasOne(si => si.ShoppingCart)
          .WithMany(s => s.ShoppingCartItems);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.User)
          .WithMany(u => u.Orders);

      modelBuilder.Entity<OrderItem>()
          .HasOne(oi => oi.Order)
          .WithMany(o => o.OrderItems);
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<JwtToken> JwtTokens { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentCard> PaymentCards { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<User> Users { get; set; }
  }
}