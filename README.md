# Auth
Bu proje, .NET teknolojisi kullanılarak geliştirilmiş Kullanıcı CRUD işlemlerinin örneklerinin bulunduğu bir web uygulamasıdır. Kullanıcının parolasını şifreleme, parola sıfırlama ve mail gönderimi gibi işlemleri gerçekleştirir.

## Teknolojiler

- .NET Core
- Entity Framework Core
- MySQL
- JWT
- C#

## Klasör Yapısı

├── Auth.sln                       # Proje çözüm dosyası                                                                                                          
├── AppBusiness/                   # İş katmanı (Business Layer)                                              
│   ├── AppBusiness.csproj         # Proje dosyası                
│   ├── Abstract/                  # Arayüz ve Abstraction sınıfları                        
│   │   └── IAuthService.cs        # Authentication arayüzü                  
│   ├── Concrete/                  # Gerçekleştirim (Implementation) sınıfları                    
│   │   └── AuthService.cs         # Authentication servisi               
│   └── Validation/                # Doğrulama işlemleri            
│       ├── Kullanici/             # Kullanıcı doğrulamaları             
│       │   └── KullaniciUpdateValidator.cs           
│       └── LoginRegister/         # Giriş ve kayıt doğrulamaları             
│           ├── KullaniciLoginValidator.cs           
│           └── KullaniciRegisterValidator.cs           
├── AppCore/                       # Çekirdek (Core) katmanı          
│   ├── AppCore.csproj             # Proje dosyası         
│   ├── Entities/                  # Varlık sınıfları (Entities)            
│   │   ├── Audit.cs               # Denetim sınıfı          
│   │   ├── IEntity.cs             # Temel varlık arayüzü             
│   │   └── ISoftDeleted.cs        # Soft delete özelliği için arayüz           
├── AppDAL/                        # Veri erişim katmanı (Data Access Layer)            
│   ├── AppDAL.csproj              # Proje dosyası            
│   ├── Configuration/             # Entity yapılandırmaları             
│   │   ├── KullaniciConfiguration.cs                
│   │   ├── RoleConfiguration.cs             
│   │   ├── UserActivityConfiguration.cs                                                                                                                                                                
│   │   └── UserRoleConfiguration.cs         
│   ├── Context/                   # Veritabanı bağlamı (DbContext)                                                                                                                           
│   │   └── AuthContext.cs               
│   ├── DTO/                       # Veri Transfer Obje'leri (DTO)       
│   │   ├── KullaniciDTO/                                     
│   │   │   ├── KullaniciGetDTO.cs                  
│   │   │   ├── KullaniciListDTO.cs               
│   │   │   ├── KullaniciPwUpdateDTO.cs                
│   │   │   └── KullaniciUpdateDTO.cs               
│   │   └── LoginRegisterDTO/               
│   │       ├── KullaniciLoginDTO.cs                
│   │       ├── KullaniciRegisterDTO.cs                    
│   │       └── LogoutRequestDTO.cs                   
│   ├── Entity/                    # Veritabanı varlıkları              
│   │   ├── Kullanici.cs             
│   │   ├── PwReset.cs               
│   │   ├── Role.cs           
│   │   ├── UserActivity.cs           
│   │   └── UserRole.cs            
│   ├── LoginSecurity/             # Giriş güvenliği işlemleri              
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
│       ├── HashingHelper.cs          
│       ├── ITokenHelper.cs          
│       └── TokenHelper.cs             
├── AppWebAPI/                     # Web API katmanı              
│   ├── appsettings.Development.json                                    
│   ├── appsettings.json          
│   ├── AppWebAPI.csproj             
│   ├── Program.cs                 # Uygulama giriş noktası          
│   ├── Startup.cs                 # API yapılandırması             
│   ├── Controllers/               # API Controller'ları            
│       ├── AuthController.cs           
│       └── PasswordResetController.cs        