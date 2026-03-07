using DevConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Context;

public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Recruiter> Recruiters { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}