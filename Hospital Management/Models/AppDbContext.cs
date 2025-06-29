﻿using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
    }
}
