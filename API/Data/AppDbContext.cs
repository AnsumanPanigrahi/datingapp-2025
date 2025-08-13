using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> Users{ get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

}
