using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.LoginSecurity.Extension
{
    public static class RoleExtension
    {
        public static void AddRole(this ICollection<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var item in roles) claims.Add(new Claim(PayloadRoleIdentifier.Role, item));
        }
    }
}
