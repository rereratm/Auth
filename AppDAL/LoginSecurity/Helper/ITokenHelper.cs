using AppDAL.Entity;
using AppDAL.LoginSecurity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.LoginSecurity.Helper
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(Kullanici kullanici, IEnumerable<Role> userRoles);
    }
}
