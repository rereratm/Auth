using AppDAL.Context;
using AppDAL.Entity;
using AppDAL.LoginSecurity.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace AppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly AuthContext _dbContext;

        public PasswordResetController(AuthContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("request/{email}")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            // Mail formatını kontrol et
            if (!IsValidEmail(email))
            {
                return BadRequest("Invalid email format.");
            }

            var user = await _dbContext.kullanicilar.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Şifre sıfırlama kodu oluştur
            var resetCode = GenerateResetCode();

            var pwReset = new PwReset
            {
                UserId = user.Id, // Şifre sıfırlama kodunun kullanıcısı
                ResetCode = resetCode,
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1)
            };

            _dbContext.pwresets.Add(pwReset);
            await _dbContext.SaveChangesAsync();

            await _dbContext.SaveChangesAsync();

            // Şifre sıfırlama linkini içeren e-posta gönder
            var success = await SendResetCodeEmail(email, resetCode);

            if (success)
            {
                return Ok("Password reset code has been sent to your email.");
            }
            else
            {
                return StatusCode(500, "Failed to send the email.");
            }
        }

        private async Task<bool> SendResetCodeEmail(string email, string resetCode)
        {
            var apiKey = "yourapikey";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("yourmail", "yourcompany");
            var subject = "Password Reset";
            var to = new EmailAddress(email);
            var plainTextContent = $"Your password reset code is: {resetCode}";
            var htmlContent = $"<strong>Your password reset code is: {resetCode}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateResetCode()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(string email, string resetCode, string password, string confirmPassword)
        {
            // Mail formatını kontrol et
            if (!IsValidEmail(email))
            {
                return BadRequest("Invalid email format.");
            }

            var user = await _dbContext.kullanicilar.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var pwReset = new PwReset
            {
                UserId = user.Id, // Şifre sıfırlama kodunun kullanıcısı
                ResetCode = resetCode,
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1)
            };

            // Şifre sıfırlama kodunun süresini kontrol et
            if (pwReset.ResetCodeExpiration <= DateTime.UtcNow)
            {
                return BadRequest("Reset code has expired.");
            }

            // Şifre sıfırlama kodunu doğrula
            if (pwReset.ResetCode != resetCode)
            {
                return BadRequest("Invalid reset code.");
            }

            // Yeni şifre ve şifre tekrarını kontrol et
            if (password != confirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Şifre sıfırlama kodu alanlarını sıfırla
            // Şifre sıfırlama kodunu alanları sıfırla
            var lastPwReset = await _dbContext.pwresets
                .Where(p => p.UserId == pwReset.UserId)
                .OrderByDescending(p => p.Id) // Sıralama işlemi
                .FirstOrDefaultAsync();

            if (lastPwReset != null)
            {
                _dbContext.pwresets.Remove(lastPwReset);
                await _dbContext.SaveChangesAsync();
            }



            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Password has been reset successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Hata durumunda ilgili hata mesajını yakalayarak işleyin
                var innerException = ex.InnerException as MySqlException;
                if (innerException != null)
                {
                    // Hata kodunu ve mesajını kontrol edin
                    if (innerException.Number == 1048 && innerException.Message.Contains("ResetCode"))
                    {
                        return BadRequest("An error occurred while resetting the password.");
                    }
                    // Diğer MySQL hata durumlarını burada işleyebilirsiniz

                    // Genel hata durumlarını işlemek için aşağıdaki gibi bir dönüş yapabilirsiniz
                    return StatusCode(500, "An error occurred while resetting the password.");
                }

                // Diğer hata durumlarını burada işleyebilirsiniz
                throw;
            }

        }
    }
}
