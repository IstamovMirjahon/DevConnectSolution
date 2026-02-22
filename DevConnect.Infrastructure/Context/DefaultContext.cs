using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Context;

public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}