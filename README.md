
# Auth
Bu proje, .NET teknolojisi kullanılarak geliştirilmiş Kullanıcı CRUD işlemlerinin örneklerinin bulunduğu bir web uygulamasıdır. Kullanıcının parolasını şifreleme, parola sıfırlama ve mail gönderimi gibi işlemleri gerçekleştirir.

## Teknolojiler

- .NET Core
- Entity Framework Core
- MySQL
- JWT
- C#

├── Auth.sln                      # Proje çözüm dosyası, Visual Studio'da projenin ana çözüm dosyasıdır.
├── .gitignore                     # Git ayar dosyası, projede hangi dosyaların versiyon kontrolüne alınmayacağını belirler.
├── README.md                      # Proje hakkında açıklama, projenin ne yaptığı, nasıl kurulduğu ve kullanımına dair bilgileri içerir.
├── AppBusiness/                   # İş katmanını içerir. Uygulamanın iş mantığını ve doğrulama işlemlerini burada bulabilirsiniz.
│   ├── AppBusiness.csproj         # AppBusiness katmanı için proje dosyası, iş katmanı derlemelerini yönetir.
│   ├── Abstract/                  # Abstraction ve interface tanımlamaları içerir, iş katmanındaki servislerin kontratları burada bulunur.
│   │   └── IAuthService.cs        # Authentication servisi interface'i, kullanıcı doğrulama işlemlerinin sözleşmesini belirler.
│   ├── Concrete/                  # Gerçekleştirim (implementation) sınıflarını içerir, interface'lerin implementasyonları buradadır.
│   │   └── AuthService.cs         # Authentication servisi, IAuthService'in gerçekleştirilmiş hali.
│   └── Validation/                # Kullanıcı giriş/çıkış doğrulamalarını içerir, FluentValidation ile doğrulama kuralları buradadır.
│       ├── Kullanici/             
│       │   └── KullaniciUpdateValidator.cs   # Kullanıcı güncelleme işlemleri için doğrulama kurallarını içerir.
│       └── LoginRegister/         
│           ├── KullaniciLoginValidator.cs    # Kullanıcı giriş işlemi için doğrulama kurallarını içerir.
│           └── KullaniciRegisterValidator.cs # Kullanıcı kayıt işlemi için doğrulama kurallarını içerir.
├── AppCore/                       # Projenin temel yapılarını içerir. Bu katmanda tüm projede kullanılan ortak yapılar yer alır.
│   ├── AppCore.csproj             # AppCore katmanı için proje dosyası.
│   ├── Entities/                  # Varlık sınıflarını (Entities) içerir, proje boyunca kullanılan temel modeller buradadır.
│   │   ├── Audit.cs               # Denetim (audit) için kullanılan yapılar, veri değişikliklerinin takibi için kullanılır.
│   │   ├── IEntity.cs             # Tüm entity'lerin uygulaması gereken temel arayüz, her entity'nin bir ID'si olması gerektiğini belirtir.
│   │   └── ISoftDeleted.cs        # Soft delete yapılacak entity'ler için interface, veritabanından silinmeyen ama pasif hale getirilen kayıtları yönetir.
├── AppDAL/                        # Veri erişim katmanını (Data Access Layer) içerir, veritabanı ile olan etkileşimleri bu katman gerçekleştirir.
│   ├── AppDAL.csproj              # AppDAL katmanı için proje dosyası.
│   ├── Configuration/             # Entity yapılandırmalarını içerir, entity'lerin veritabanı tablolarına nasıl haritalandığını ayarlar.
│   │   ├── KullaniciConfiguration.cs          # Kullanici entity'sinin veritabanı yapılandırması.
│   │   ├── RoleConfiguration.cs               # Role entity'sinin veritabanı yapılandırması.
│   │   ├── UserActivityConfiguration.cs       # UserActivity entity'sinin veritabanı yapılandırması.
│   │   └── UserRoleConfiguration.cs           # UserRole entity'sinin veritabanı yapılandırması.
│   ├── Context/                   # Veritabanı bağlamı (DbContext), veritabanı bağlantılarını ve entity setlerini yönetir.
│   │   └── AuthContext.cs         # Projenin veritabanı bağlantısını yöneten sınıf, Entity Framework Core üzerinden veritabanı işlemlerini sağlar.
│   ├── DTO/                       # Veri Transfer Obje'leri (DTO), veriler arasında taşınan bilgi objelerini içerir.
│   │   ├── KullaniciDTO/          # Kullanıcıya özgü DTO sınıflarını içerir.
│   │   │   ├── KullaniciGetDTO.cs             # Kullanıcı bilgilerini almak için kullanılan DTO.
│   │   │   ├── KullaniciListDTO.cs            # Kullanıcı listesini almak için kullanılan DTO.
│   │   │   ├── KullaniciPwUpdateDTO.cs        # Kullanıcı parolası güncelleme işlemleri için kullanılan DTO.
│   │   │   └── KullaniciUpdateDTO.cs          # Kullanıcı güncelleme işlemleri için kullanılan DTO.
│   │   └── LoginRegisterDTO/      # Giriş/Kayıt işlemlerine özgü DTO sınıflarını içerir.
│   │       ├── KullaniciLoginDTO.cs           # Kullanıcı giriş bilgilerini almak için kullanılan DTO.
│   │       ├── KullaniciRegisterDTO.cs        # Kullanıcı kayıt bilgilerini almak için kullanılan DTO.
│   │       └── LogoutRequestDTO.cs            # Kullanıcı çıkış işlemi için kullanılan DTO.
│   ├── Entity/                    # Veritabanı varlıkları (Entities), veritabanında karşılık gelen tabloları temsil eder.
│   │   ├── Kullanici.cs           # Kullanıcı entity'si.
│   │   ├── PwReset.cs             # Parola sıfırlama taleplerini yönetmek için kullanılan entity.
│   │   ├── Role.cs                # Rol entity'si, kullanıcı yetkilerini yönetir.
│   │   ├── UserActivity.cs        # Kullanıcı aktivitelerini takip eden entity.
│   │   └── UserRole.cs            # Kullanıcı rollerini yönetmek için kullanılan entity.
│   ├── LoginSecurity/             # Giriş güvenliği ile ilgili sınıflar, JWT token üretimi ve şifreleme işlemleri burada yapılır.
│   │   ├── Encryption/            # Şifreleme işlemleri.
│   │   │   ├── SecurityKeyHelper.cs            # Güvenlik anahtarı oluşturma işlemleri.
│   │   │   └── SigningCredentialsHelper.cs     # İmzalama kimlik bilgisi oluşturma işlemleri.
│   │   ├── Entity/                # JWT token ile ilgili entity sınıfları.
│   │   │   ├── AccessToken.cs                   # Erişim token'ı entity'si.
│   │   │   └── TokenOptions.cs                  # Token seçenekleri.
│   │   ├── Extension/             # Genişletme metotları (extension methods), ek işlevsellikler sağlar.
│   │   │   ├── PayloadRoleIdentifier.cs         # Token'dan rol bilgisi çıkaran sınıf.
│   │   │   └── RoleExtension.cs                 # Rol bilgisi için genişletme metodu.
│   │   └── Helper/                # Yardımcı sınıflar, çeşitli şifreleme ve token üretme işlemleri için kullanılır.
│   │       ├── HashingHelper.cs                 # Parola hashleme işlemleri.
│   │       ├── ITokenHelper.cs                  # Token üretim yardımcı interface'i.
│   │       └── TokenHelper.cs                   # Token üretim işlemlerini yapan sınıf.
├── AppWebAPI/                     # Web API katmanı, uygulamanın dış dünya ile iletişimini sağlayan API katmanı.
│   ├── appsettings.Development.json # Geliştirme ortamı için yapılandırma dosyası.
│   ├── appsettings.json             # Genel yapılandırma dosyası, çeşitli ayarları içerir.
│   ├── AppWebAPI.csproj             # Web API için proje dosyası.
│   ├── Program.cs                   # Uygulamanın giriş noktası, ana uygulamanın başlatıldığı dosya.
│   ├── Startup.cs                   # API yapılandırmasının yapıldığı sınıf, servisler ve middleware'ler burada eklenir.
│   ├── Controllers/                 # API Controller'ları, dış dünyaya sunulan API endpoint'leri burada tanımlanır.
│   │   ├── AuthController.cs         # Kullanıcı giriş, kayıt, doğrulama işlemleri için API controller'ı.
│   │   └── PasswordResetController.cs # Parola sıfırlama işlemleri için API controller'ı.




  
