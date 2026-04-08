# SW.API İçin Çalışma Özeti

# Ne yapıldığının kısa özeti

Bu çalışmada Akıllı Depo Yönetimi modülünün backend tarafı, sıfırdan ve katmanlı mimari prensipleri esas alınarak geliştirilmiştir. Ürün tanımlama, ürün listeleme, ürün güncelleme, soft delete, stok giriş, stok çıkış ve dashboard özet verileri gibi temel iş akışları .NET 9.0 tabanlı ASP.NET Core Web API ile hayata geçirilmiştir. Geliştirme sürecinde sadece çalışır kod üretmek değil, aynı zamanda sürdürülebilir, genişletilebilir ve kurumsal projelere adapte edilebilir bir servis omurgası kurmak hedeflenmiştir. Bu nedenle Controller → Manager → Repository → Entity (DbContext) akışı korunmuş, iş kuralları manager katmanında toplanmış, veri erişimi repository katmanına ayrılmış ve veritabanı yapısı Entity Framework Core Migration ile yönetilmiştir. Özellikle multi-tenant CompanyId güvenliği, soft delete yaklaşımı, server-side pagination, unique index kullanımı ve stok hareketlerinin transaction bazlı izlenmesi gibi kararlarla sistem hem veri bütünlüğü hem de operasyonel kontrol açısından güçlü bir temele oturtulmuştur.

# Kullanılan teknolojiler ve versiyonları

.NET 9.0 (ASP.NET Core Web API)
Entity Framework Core
MS SQL Server
Swagger / Swashbuckle
C#
Katmanlı mimari (Controller / Manager / Repository / Entity)

# Karşılaşılan sorunlar ve çözüm yolları

Duplicate ürün kodu senaryosunda veri tabanı constraint hatasının doğrudan kullanıcıya yansıması : Aynı CompanyId + ProductCode kombinasyonu ikinci kez eklenmek istendiğinde, veritabanı unique index’i doğru şekilde koruma sağladı; ancak ilk aşamada bu hata kullanıcı dostu bir response’a çevrilmiyordu. Bu durum manager katmanında ön kontrol ve ek hata yakalama yaklaşımı ile iyileştirildi. Böylece veri bütünlüğü korunurken API davranışı da daha kontrollü hale getirildi.

Stok işlemlerinde sadece miktar güncellemek yerine iş akışı bütünlüğünü koruma ihtiyacı : Stok giriş ve çıkış işlemlerinde sadece CurrentStock alanını değiştirmek yeterli görülmedi. İşveren açısından denetlenebilirlik ve kurumsal sürdürülebilirlik önemli olduğu için her stok hareketinin ayrıca StockTransactions tablosuna yazılması sağlandı. Böylece sistem sadece güncel durumu tutan değil, aynı zamanda geçmiş hareketleri izlenebilen, audit edilebilir bir yapıya dönüştürüldü.

# Mimari kararlar ve nedenleri

Katmanlı yapı tercih edildi çünkü iş kurallarını controller seviyesinde dağınık bırakmak yerine manager katmanında toplamak, bakım ve genişleme süreçlerinde ciddi avantaj sağlar.
Entity Framework Core + Migration kullanıldı çünkü veritabanı şemasının kod ile senkron yönetilmesi, ortamlar arası geçişte kontrol ve izlenebilirlik sağlar.
Soft delete yaklaşımı seçildi çünkü kurumsal sistemlerde verinin tamamen fiziksel silinmesi çoğu zaman risklidir; geri izleme ve raporlama açısından kayıtların korunması değerlidir.
Server-side pagination uygulandı çünkü büyük veri setlerinde performansın frontend’e bırakılması yerine backend tarafından yönetilmesi daha doğru ve ölçeklenebilir bir yaklaşımdır.
Multi-tenant CompanyId kontrolü merkeze alındı çünkü farklı şirket verilerinin birbirinden izole tutulması bu tip modüllerde güvenlik ve veri bütünlüğü açısından kritik önemdedir.
Stok hareketleri ayrı transaction tablosunda tutuldu çünkü anlık stok bilgisinin yanında geçmiş hareketlerin de izlenmesi, sistemin kurumsal seviyede kullanılabilirliğini artırır.

# Yapay zeka hangi aşamalarda kullanıldı

Katmanlı yapı ve iş akışlarının daha hızlı taslaklanmasında destek amaçlı kullanıldı.
DTO, repository ve manager kontratlarının ilk tasarım hızını artırmak için kullanıldı.
Edge case senaryoları ve hata yönetimi yaklaşımını gözden geçirmek için yardımcı araç olarak değerlendirildi.

# Geliştirilebilir 5 Madde

BaseResponseDto yerine generic BaseResponseDto<T> yapısına geçilerek response standardı daha güçlü hale getirilebilir.
Validasyon kuralları manager içinden ayrılıp FluentValidation ile daha modüler hale getirilebilir.
Ortak transaction yönetimi için UnitOfWork yaklaşımı eklenebilir.
Yetkilendirme ve tenant doğrulama için merkezi bir authorization/policy katmanı kurulabilir.
Warehouse, StorageLocation ve ProductCategory için ayrı yönetim endpoint’leri eklenerek sistemin operasyonel kapsamı büyütülebilir.
