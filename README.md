
# Auth
Bu proje, .NET teknolojisi kullanılarak geliştirilmiş Kullanıcı CRUD işlemlerinin örneklerinin bulunduğu bir web uygulamasıdır. Kullanıcının parolasını şifreleme, parola sıfırlama ve mail gönderimi gibi işlemleri gerçekleştirir.

## Teknolojiler

- .NET Core
- Entity Framework Core
- MySQL
- JWT
- C#

├── Auth.sln
├── AppBusiness/
│   ├── AppBusiness.csproj
│   ├── Abstract/
│   │   └── IAuthService.cs
│   ├── Concrete/
│   │   └── AuthService.cs
│   └── Validation/
│       ├── Kullanici/
│       │   └── KullaniciUpdateValidator.cs
│       └── LoginRegister/
│           ├── KullaniciLoginValidator.cs
│           └── KullaniciRegisterValidator.cs
├── AppCore/
│   ├── AppCore.csproj
│   ├── Entities/
│   │   ├── Audit.cs
│   │   ├── IEntity.cs
│   │   └── ISoftDeleted.cs
├── AppDAL/
│   ├── AppDAL.csproj
│   ├── Configuration/
│   │   ├── KullaniciConfiguration.cs
│   │   ├── RoleConfiguration.cs
│   │   ├── UserActivityConfiguration.cs
│   │   └── UserRoleConfiguration.cs
│   ├── Context/
│   │   └── AuthContext.cs
│   ├── DTO/
│   │   ├── KullaniciDTO/
│   │   │   ├── KullaniciGetDTO.cs
│   │   │   ├── KullaniciListDTO.cs
│   │   │   ├── KullaniciPwUpdateDTO.cs
│   │   │   └── KullaniciUpdateDTO.cs
│   │   └── LoginRegisterDTO/
│   │       ├── KullaniciLoginDTO.cs
│   │       ├── KullaniciRegisterDTO.cs
│   │       └── LogoutRequestDTO.cs
│   ├── Entity/
│   │   ├── Kullanici.cs
│   │   ├── PwReset.cs
│   │   ├── Role.cs
│   │   ├── UserActivity.cs
│   │   └── UserRole.cs
│   ├── LoginSecurity/
│   │   ├── Encryption/
│   │   │   ├── SecurityKeyHelper.cs
│   │   │   └── SigningCredentialsHelper.cs
│   │   ├── Entity/
│   │   │   ├── AccessToken.cs
│   │   │   └── TokenOptions.cs
│   │   ├── Extension/
│   │   │   ├── PayloadRoleIdentifier.cs
│   │   │   └── RoleExtension.cs
│   │   └── Helper/
│   │       ├── HashingHelper.cs
│   │       ├── ITokenHelper.cs
│   │       └── TokenHelper.cs
├── AppWebAPI/
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── AppWebAPI.csproj
│   ├── Program.cs
│   ├── Startup.cs
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── PasswordResetController.cs



  
