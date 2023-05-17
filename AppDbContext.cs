namespace ReproduceWolverineIssue;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> context)
        : base(context)
    { }
}