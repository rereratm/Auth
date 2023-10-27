using AppDAL.DTO.LoginRegisterDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBusiness.Validation.LoginRegister
{
    public class KullaniciLoginValidator : AbstractValidator<KullaniciLoginDTO>
    {
        public KullaniciLoginValidator()
        {
            RuleFor(p => p.KullaniciAdi).NotEmpty().WithMessage("Kullanici Adi bos bırakılamaz.");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Sifre bos bırakılamaz.");
        }
    }
}
