using AppBusiness.Abstract;
using AppBusiness.Validation.Kullanici;
using AppBusiness.Validation.LoginRegister;
using AppDAL.DTO.KullaniciDTO;
using AppDAL.DTO.LoginRegisterDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]

        public async Task<ActionResult> Register(KullaniciRegisterDTO kullaniciRegisterDTO)
        {
            var list = new List<string>();
            var validator = new KullaniciRegisterValidator();
            var validationResults = validator.Validate(kullaniciRegisterDTO);
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors)
                {
                    list.Add(error.ErrorMessage);
                }
                return Ok(new { code = StatusCode(1002), message = list, type = "error" });
            }
            try
            {
                var registerResult = await _authService.RegisterAsync(kullaniciRegisterDTO);
                if (registerResult > 0)
                {
                    return Ok(new { code = StatusCode(1000), message = "Kullanıcı kaydı başarılı", type = "success" });
                }
                return Ok(new { code = StatusCode(1001), message = "Kullanıcı kaydı başarısız", type = "error" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(KullaniciLoginDTO kullaniciLoginDTO)
        {
            var list = new List<string>();
            var validator = new KullaniciLoginValidator();
            var validationResults = validator.Validate(kullaniciLoginDTO);
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors)
                {
                    list.Add(error.ErrorMessage);
                }
                return Ok(new { code = StatusCode(1002), message = list, type = "error" });
            }
            var currentKullanici = await _authService.GetLoginKullaniciAsync(kullaniciLoginDTO);

            if (currentKullanici == null)
            {
                return Ok(new { code = StatusCode(1001), message = "Kullanıcı adı veya şifre yanlış", type = "error" });
            }

            var accessToken = await _authService.CreateAccessTokenAsync(currentKullanici);
            return Ok(accessToken);
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<KullaniciGetDTO>> GetKullaniciById(int id)
        {
            var list = new List<string>();
            if (id <= 0)
            {
                list.Add("Geçersiz ID!");
                return Ok(new { code = StatusCode(1002), message = list, type = "error" });
            }
            try
            {
                var currentKullanici = await _authService.GetUserAsync(id);
                if (currentKullanici == null)
                {
                    list.Add("Kayıt Bulunamadı!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
                else
                {
                    return currentKullanici;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUserList")]
        public async Task<ActionResult<List<KullaniciListDTO>>> GetKullaniciList()
        {
            try
            {
                return Ok(await _authService.GetUserList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         [HttpPut("UpdateKullanici/{id}")]
         public async Task<ActionResult<string>> UpdateKullanici(int id, KullaniciUpdateDTO kullaniciUpdateDTO)
         {
             var list = new List<string>();
             var validator = new KullaniciUpdateValidator();
             var validationResults = validator.Validate(kullaniciUpdateDTO);
             if (!validationResults.IsValid)
             {
                 foreach (var error in validationResults.Errors)
                 {
                     list.Add(error.ErrorMessage);
                     return Ok(new { code = StatusCode(1002), message = list, type = "error" });
                 }
             }
             try
             {
                 var result = await _authService.UpdateKullanici(id, kullaniciUpdateDTO);
                 if (result > 0)
                 {
                     list.Add("Güncelleme Başarılı!");
                     return Ok(new { code = StatusCode(1000), message = list, type = "success" });
                 }
                 else if (result == -1)
                 {
                     list.Add("Kayıt Bulunamadı!");
                     return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                 }
                 else
                 {
                     list.Add("Güncelleme Başarısız!");
                     return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                 }
             }
             catch (Exception ex)
             {
                 return BadRequest(ex.Message);
             }
         }
        
        [HttpDelete("DeleteKullanici/{id}")]
        public async Task<ActionResult<string>> DeleteKullanici(int id)
        {
            var list = new List<string>();
            try
            {
                var result = await _authService.DeleteKullanici(id);
                if (result > 0)
                {
                    list.Add("Silme İşlemi Başarılı!");
                    return Ok(new { code = StatusCode(1000), message = list, type = "success" });
                }
                else if (result == -1)
                {
                    list.Add("Kayıt Bulunamadı!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
                else
                {
                    list.Add("Silme İşlemi Başarısız!");
                    return Ok(new { code = StatusCode(1002), message = list, type = "error" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
       
        [HttpPut("UpdatePassword/{kullaniciAdi}")]
        public async Task<ActionResult<int>> UpdatePassword(string kullaniciAdi, KullaniciPwUpdateDTO kullaniciPwUpdateDTO)
        {
            var list = new List<string>();
            try
            {
                var result = await _authService.UpdatePassword(kullaniciAdi, kullaniciPwUpdateDTO);
                if (result > 0)
                {
                    list.Add("Güncelleme Başarılı!");
                    return Ok(new { code = StatusCode(1000), message = list, type = "success" });
                }
                else if (result == -1)
                {
                    list.Add("Kayıt Bulunamadı!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
                else
                {
                    list.Add("Güncelleme Başarısız!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
        [HttpPut("UpdateKullanici/{ekipNetNo}")]
        public async Task<ActionResult<int>> UpdateKullaniciByEkipNetNo(string ekipNetNo, KullaniciUpdateDTO kullaniciUpdateDTO)
        {
            var list = new List<string>();
            var validator = new KullaniciUpdateValidator();
            var validationResults = validator.Validate(kullaniciUpdateDTO);
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors)
                {
                    list.Add(error.ErrorMessage);
                    return Ok(new { code = StatusCode(1002), message = list, type = "error" });
                }
            }
            try
            {
                var result = await _authService.UpdateKullaniciByEkipNetNo(ekipNetNo, kullaniciUpdateDTO);
                if (result > 0)
                {
                    list.Add("Güncelleme Başarılı!");
                    return Ok(new { code = StatusCode(1000), message = list, type = "success" });
                }
                else if (result == -1)
                {
                    list.Add("Kayıt Bulunamadı!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
                else
                {
                    list.Add("Güncelleme Başarısız!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUsersByFirmaId/{firmaId}")]
        public async Task<ActionResult<List<KullaniciListDTO>>> GetUsersByFirmaId(int firmaId)
        {
            try
            {
                var users = await _authService.GetUsersByFirmaId(firmaId);
                if (users.Count == 0)
                {
                    return Ok(new { code = StatusCode(1001), message = "firmaId ye göre kullanici bulunamadi", type = "error" });
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         [HttpGet("GetUserByEkipNetNo/{ekipnetno}")]
        public async Task<ActionResult<List<KullaniciGetDTO>>> GetUserByEkipNetNo(string ekipnetno)
        {
            var list = new List<string>();
            if (ekipnetno == null)
            {
                list.Add("Geçersiz EkipNetNo!");
                return Ok(new { code = StatusCode(1002), message = list, type = "error" });
            }
            try
            {
                var currentUser = await _authService.GetUserByEkipNetNo(ekipnetno);
                if (currentUser == null)
                {
                    list.Add("Kayıt Bulunamadı!");
                    return Ok(new { code = StatusCode(1001), message = list, type = "error" });
                }
                else
                {
                    return currentUser;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        */
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout([FromBody] LogoutRequestDTO request)
        {
            try
            {
                await _authService.LogoutAsync(request.UserId);
                return Ok(new { code = StatusCode(1000), message = "Oturum başarıyla sonlandırıldı", type = "success" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { code = StatusCode(1001), message = ex.Message, type = "error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = StatusCode(1002), message = "Oturum kapatma sırasında bir hata oluştu.", type = "error" });
            }
        }
    }
}
