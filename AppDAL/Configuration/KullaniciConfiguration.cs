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
    public class KullaniciConfiguration : IEntityTypeConfiguration<Kullanici>
    {
        public void Configure(EntityTypeBuilder<Kullanici> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AdSoyad).HasMaxLength(75);

            builder.Property(x => x.Unvan).HasMaxLength(20);

            builder.Property(x => x.Email).HasMaxLength(50);

            builder.Property(x => x.Adres).HasMaxLength(200);

            builder.Property(x => x.TelNo).HasMaxLength(20);

            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.PasswordSalt).IsRequired();

        }
    }
}
