using AppCore.Entities;
using AppDAL.DTO.KullaniciDTO;
using AppDAL.DTO.LoginRegisterDTO;
using AppDAL.Entity;
using AppDAL.LoginSecurity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AppBusiness.Abstract
{
    public interface IAuthService : IEntity
    {
        public Task<AccessToken> CreateAccessTokenAsync(Kullanici kullanici);
        public Task<Kullanici> GetLoginKullaniciAsync(KullaniciLoginDTO kullaniciLoginDto);
        public Task<int> RegisterAsync(KullaniciRegisterDTO kullaniciRegisterDto);
        public Task<KullaniciGetDTO> GetUserAsync(int id);
        public Task<List<KullaniciListDTO>> GetUserList();
        public Task<int> UpdateKullanici(int id, KullaniciUpdateDTO kullaniciUpdateDTO);
        public Task<int> UpdatePassword(string ekipNetNo, KullaniciPwUpdateDTO kullaniciPwUpdateDTO);
        public Task<int> DeleteKullanici(int id);
        public Task LogoutAsync(int userId);
    }
}
