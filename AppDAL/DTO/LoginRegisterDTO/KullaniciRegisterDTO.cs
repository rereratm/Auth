using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.DTO.LoginRegisterDTO
{
    public class KullaniciRegisterDTO
    {
        public string AdSoyad { get; set; }
        public string Unvan { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string KullaniciAdi { get; set; }
        public string Password { get; set; }
        public int UserRole { get; set; }
    }
}
