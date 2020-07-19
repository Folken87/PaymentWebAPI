using Microsoft.EntityFrameworkCore;

public class PaymentContext : DbContext
{
    public PaymentContext(DbContextOptions<PaymentContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentActions> PaymentHistory { get; set; }
} 