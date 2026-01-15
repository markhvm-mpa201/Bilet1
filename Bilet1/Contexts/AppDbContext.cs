using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bilet1.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<MemberPosition> MemberPositions { get; set; }

    public DbSet<TeamMember> TeamMembers { get; set; }

}
