using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entity
{
    public class PwReset : Audit, IEntity
    {
        public int Id { get; set; }
        public string ResetCode { get; set; }
        public DateTime? ResetCodeExpiration { get; set; }
        public int UserId { get; set; }
    }
}
