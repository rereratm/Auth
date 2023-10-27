using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.DTO.KullaniciDTO
{
    public class KullaniciListDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Unvan { get; set; }
        public string TelNo { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string KullaniciAdi { get; set; }

    }
}
