using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entity
{
    public class Kullanici : Audit, IEntity, ISoftDeleted
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Unvan { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string KullaniciAdi { get; set; }
   
        //Şifre direkt olarak değil şifrelenmiş halde veritabanında tutulur.
        //JWT de user kısmını kullanıcıya verdik.
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
      
        public ICollection<UserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; }
    }
}
