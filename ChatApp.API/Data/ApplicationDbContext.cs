using ChatApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }



    public DbSet<User> Users { get; set; }

}
