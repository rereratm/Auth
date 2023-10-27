using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entity
{
    public class UserRole : Audit, IEntity, ISoftDeleted
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici KullaniciFK { get; set; }
        public int RoleId { get; set; }
        public Role RoleFK { get; set; }
        public bool IsDeleted { get; set; }
    }
}
