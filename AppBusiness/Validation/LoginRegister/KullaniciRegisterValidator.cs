using AppDAL.DTO.LoginRegisterDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBusiness.Validation.LoginRegister
{
    public class KullaniciRegisterValidator : AbstractValidator<KullaniciRegisterDTO>
    {
        public KullaniciRegisterValidator()
        {
            RuleFor(p => p.AdSoyad).NotEmpty().WithMessage("AdSoyad boş bırakılamaz.").MaximumLength(100).WithMessage("En fazla 100 karakter girilmelidir.");
            RuleFor(p => p.Unvan).NotEmpty().WithMessage("Unvan boş geçilemez.").MaximumLength(100).WithMessage("En fazla 100 karakter girilmelidir.");
            RuleFor(p => p.TelNo).NotEmpty().WithMessage("Telefon no boş bırakılamaz.").MaximumLength(20).WithMessage("En fazla 20 karakter girilebilir");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email boş bırakılamaz.");
            RuleFor(p => p.Adres).NotEmpty().WithMessage("Adres boş bırakılamaz.");
            RuleFor(p => p.KullaniciAdi).NotEmpty().WithMessage("Kullanıcı Adı boş bırakılamaz!");
        }

    }
}
