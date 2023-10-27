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
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.KullaniciId).IsRequired();
            builder.Property(p => p.RoleId).IsRequired();

            builder.HasOne(p => p.RoleFK).WithMany(p => p.UserRoles).HasForeignKey(p => p.RoleId);
            builder.HasOne(p => p.KullaniciFK).WithMany(p => p.UserRoles).HasForeignKey(p => p.KullaniciId);
        }
    }
}
