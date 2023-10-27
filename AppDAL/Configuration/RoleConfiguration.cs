using AppDAL.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();

            builder.HasData(new Role { Id = 1, Name = "Admin" });
            builder.HasData(new Role { Id = 2, Name = "Kullanıcı" });
            builder.HasData(new Role { Id = 3, Name = "Yönetici" });
            builder.HasData(new Role { Id = 4, Name = "Bakanlık" });
        }
    }
}
