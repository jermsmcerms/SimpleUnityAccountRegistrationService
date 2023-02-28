using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Backend.Context;
public class GameDbContext : DbContext {
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=game.sqlite");
}
