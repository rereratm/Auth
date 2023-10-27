using AppDAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Context
{
    public class AuthContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost; Port=3306; Database=auth; Uid=root; Pwd=123456789;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        public DbSet<Kullanici> kullanicilar { get; set; }
        public DbSet<UserActivity> useractivities { get; set; }
        public DbSet<PwReset> pwresets { get; set; }
        public DbSet<UserRole> userroles { get; set; }
        public DbSet<Role> roles { get; set; }
    }
}
