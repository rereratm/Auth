using AppBusiness.Abstract;
using AppDAL.Context;
using AppDAL.DTO.KullaniciDTO;
using AppDAL.DTO.LoginRegisterDTO;
using AppDAL.Entity;
using AppDAL.LoginSecurity.Entity;
using AppDAL.LoginSecurity.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBusiness.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly AuthContext _authContext;
        private readonly ITokenHelper _tokenHelper;
        public AuthService(AuthContext authContext, ITokenHelper tokenHelper)
        {
            _authContext = authContext;
            _tokenHelper = tokenHelper;
        }
        public async Task<AccessToken> CreateAccessTokenAsync(Kullanici _kullanici)
        {
            var currentUserRoles = await GetUserRolesByKullaniciIdAsync(_kullanici.Id);
            return currentUserRoles == null ? null : _tokenHelper.CreateToken(_kullanici, currentUserRoles);
        }

        private async Task<IEnumerable<Role>> GetUserRolesByKullaniciIdAsync(int kullaniciId)
        {
            return await _authContext.userroles.Where(p => !p.IsDeleted && p.KullaniciId == kullaniciId)
                .Include(p => p.RoleFK)
                .Select(p => p.RoleFK)
                .ToListAsync();
        }

        public async Task<Kullanici> GetLoginKullaniciAsync(KullaniciLoginDTO kullaniciLoginDto)
        {
            var currentKullanici = await _authContext.kullanicilar
                .Where(p => !p.IsDeleted &&  p.KullaniciAdi == kullaniciLoginDto.KullaniciAdi)
                .FirstOrDefaultAsync();

            if (currentKullanici == null)
            {
                return null; // Kullanıcı bulunamadı durumunu burada işliyoruz
            }

            var passwordMatchResult = HashingHelper.VerifyPasswordHash(kullaniciLoginDto.Password, currentKullanici.PasswordHash, currentKullanici.PasswordSalt);

            if (!passwordMatchResult)
            {
                return null; // Şifre yanlış durumunu burada işliyoruz
            }

            var lastLoginActivity = await _authContext.useractivities
                .Where(a => a.UserId == currentKullanici.Id)
                .OrderByDescending(a => a.LoginTime)
                .FirstOrDefaultAsync();

            if (lastLoginActivity == null || (DateTime.UtcNow - lastLoginActivity.LoginTime) >= TimeSpan.FromSeconds(3))
            {
                // Önceki oturumu sonlandır
                if (lastLoginActivity != null)
                {
                    await LogoutAsync(currentKullanici.Id);
                }

                // Kullanıcının yeni oturum açma etkinliğini kaydet
                _authContext.useractivities.Add(new UserActivity
                {
                    UserId = currentKullanici.Id,
                    LoginTime = DateTime.UtcNow
                });
                await _authContext.SaveChangesAsync();

                return currentKullanici;
            }
            else
            {
                throw new ApplicationException("Aynı anda birden fazla oturuma izin verilmez.");
            }
        }

        public async Task<int> RegisterAsync(KullaniciRegisterDTO kullaniciRegisterDto)
        {
            if (await _authContext.kullanicilar.AnyAsync(x => x.KullaniciAdi == kullaniciRegisterDto.KullaniciAdi))
                return -1;
            var currentTime = DateTime.Now;
            HashingHelper.CreatePasswordHash(kullaniciRegisterDto.Password, out var passwordHash, out var passwordSalt);
            var kullanici = new Kullanici
            {
                AdSoyad = kullaniciRegisterDto.AdSoyad,
                Unvan = kullaniciRegisterDto.Unvan,
                TelNo = kullaniciRegisterDto.TelNo,
                Email = kullaniciRegisterDto.Email,
                Adres = kullaniciRegisterDto.Adres,
                KullaniciAdi = kullaniciRegisterDto.KullaniciAdi,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CDate = currentTime,
                UserRoles = new List<UserRole>
                {
                    new() {RoleId = kullaniciRegisterDto.UserRole, CDate = currentTime}
                }
            };
            await _authContext.kullanicilar.AddAsync(kullanici);
            var result = await _authContext.SaveChangesAsync();
            if (result > 0)
            {
                return kullanici.Id;
            }
            else
            {
                return -1;
            }
        }
        public async Task<KullaniciGetDTO> GetUserAsync(int id)
        {
            var kullanici = await _authContext.kullanicilar.Where(p => !p.IsDeleted && p.Id == id)
                .Select(p => new KullaniciGetDTO
                {
                    Id = p.Id,
                    AdSoyad = p.AdSoyad,
                    Unvan = p.Unvan,
                    TelNo = p.TelNo,
                    Email = p.Email,
                    Adres = p.Adres,
                    KullaniciAdi = p.KullaniciAdi
                }).FirstOrDefaultAsync();
            return kullanici;
        }
        public async Task<List<KullaniciListDTO>> GetUserList()
        {
            return await _authContext.kullanicilar.Where(p => !p.IsDeleted).Select(p => new KullaniciListDTO
            {
                Id = p.Id,
                AdSoyad = p.AdSoyad,
                Unvan = p.Unvan,
                TelNo = p.TelNo,
                Email = p.Email,
                Adres = p.Adres,
                KullaniciAdi = p.KullaniciAdi
            }).ToListAsync();
        }
        public async Task<int> UpdateKullanici(int id, KullaniciUpdateDTO kullaniciUpdateDTO)
        {
            var user = await _authContext.kullanicilar.FindAsync(id);
            if (user == null)
                return -1;

            user.AdSoyad = kullaniciUpdateDTO.AdSoyad;
            user.TelNo = kullaniciUpdateDTO.TelNo;
            user.Email = kullaniciUpdateDTO.Email;
            user.Adres = kullaniciUpdateDTO.Adres;
            user.Unvan = kullaniciUpdateDTO.Unvan;
            user.KullaniciAdi = kullaniciUpdateDTO.KullaniciAdi;

            _authContext.kullanicilar.Update(user);
            var result = await _authContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdatePassword(string kullaniciAdi, KullaniciPwUpdateDTO kullaniciPwUpdateDTO)
        {
            var kullanici = await _authContext.kullanicilar.FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi);
            if (kullanici == null)
                return -1;

            if (!string.IsNullOrWhiteSpace(kullaniciPwUpdateDTO.Password))
            {
                if (!HashingHelper.VerifyPasswordHash(kullaniciPwUpdateDTO.OldPassword, kullanici.PasswordHash, kullanici.PasswordSalt))
                {
                    // return error message for incorrect old password
                    return -2;
                }
                HashingHelper.CreatePasswordHash(kullaniciPwUpdateDTO.Password, out var passwordHash, out var passwordSalt);
                kullanici.PasswordHash = passwordHash;
                kullanici.PasswordSalt = passwordSalt;

                _authContext.kullanicilar.Update(kullanici);
                var result = await _authContext.SaveChangesAsync();
                return result;
            }
            return -3;
        }

        public async Task<int> DeleteKullanici(int id)
        {
            var currentKullanici = await _authContext.kullanicilar.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if (currentKullanici != null)
            {
                currentKullanici.IsDeleted = true;
                return await _authContext.SaveChangesAsync();
            }
            return -1;
        }

   /*   Belirli bir parametreye göre update etme service i
    *   
    *   
    *      public async Task<int> UpdateKullaniciByEkipNetNo(string ekipNetNo, KullaniciUpdateDTO kullaniciUpdateDTO)
        {
            var user = await _perkonContext.kullanicilar.FirstOrDefaultAsync(u => u.EkipNetNo == ekipNetNo);
            if (user == null)
                return -1;

            // Update user properties
            user.AdSoyad = kullaniciUpdateDTO.AdSoyad;
            user.TelNo = kullaniciUpdateDTO.TelNo;
            user.Email = kullaniciUpdateDTO.Email;
            user.Adres = kullaniciUpdateDTO.Adres;
            user.DiplomaNo = kullaniciUpdateDTO.DiplomaNo;
            user.Unvan = kullaniciUpdateDTO.Unvan;
            user.KullaniciAdi = kullaniciUpdateDTO.KullaniciAdi;
            user.OdaSicilNo = kullaniciUpdateDTO.OdaSicilNo;

            _perkonContext.kullanicilar.Update(user);
            var result = await _perkonContext.SaveChangesAsync();
            return result;
        }
        }
*/

        /*
         * 
         * Belirli bir parametreye göre update etme işlemi
        public async Task<int> UpdateKullaniciByEkipNetNo(string ekipNetNo, KullaniciUpdateDTO kullaniciUpdateDTO)
        {
            var user = await _perkonContext.kullanicilar.FirstOrDefaultAsync(u => u.EkipNetNo == ekipNetNo);
            if (user == null)
                return -1;

            // Update user properties
            user.AdSoyad = kullaniciUpdateDTO.AdSoyad;
            user.TelNo = kullaniciUpdateDTO.TelNo;
            user.Email = kullaniciUpdateDTO.Email;
            user.Adres = kullaniciUpdateDTO.Adres;
            user.DiplomaNo = kullaniciUpdateDTO.DiplomaNo;
            user.Unvan = kullaniciUpdateDTO.Unvan;
            user.KullaniciAdi = kullaniciUpdateDTO.KullaniciAdi;
            user.OdaSicilNo = kullaniciUpdateDTO.OdaSicilNo;

            _perkonContext.kullanicilar.Update(user);
            var result = await _perkonContext.SaveChangesAsync();
            return result;
        }
        */
       
        public async Task LogoutAsync(int userId)
        {
            try
            {
                // Find all the user's session activities
                var sessionActivities = await _authContext.useractivities
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                // Remove all session activities from the database
                _authContext.useractivities.RemoveRange(sessionActivities);
                await _authContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Oturum kapatma sırasında bir hata oluştu.", ex);
            }
        }

    }
}

