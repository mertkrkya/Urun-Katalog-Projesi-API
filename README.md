# Mert Karakaya - Bitirme Projesi

## Projede Kullanılan Teknolojiler ve Patternler
- .NET Core 5.0
- Fluent Validation
- Migration
- Entity Framework Core
- Memory Cache
- Hangfire
- Serilog
- PostgreSQL
- IdentityServer
- JWT Web Token
- AutoMapper
- Dependency Injection
- Options Pattern

## Projeyi çalıştırmak
- Projeye ilişkin [Postman dokümanına](https://documenter.getpostman.com/view/16058133/VUqptcxm) bu linkte bulabilirsiniz.
- API projesi IIS Express ile çalıştırılarak proje başlatılır.
- Proje çalıştırıldığı zaman Swagger dokümanı açılacaktır.
- Projede veritabanı, dosya yolu, e-mail servis, ve projede kullanılan opsiyonel özellik ayarlamaları appsettings.json üzerinden güncelleştirilebilir.
- Proje SMTP servisi olarak office365 ile denenmiş ve başarılı bir sonuç alınmıştır.
- Veritabanları codefirst yaklaşımı ile oluşturulmuştur. AppDbContext ve ConfigDbContext olmak üzere iki adet context dosyası bulunmaktadır.
- Migration yöntemi kullanılarak;
```
add-migration initial -context AppDbContext
update-database -context AppDbContext
add-migration initial -context ConfigDbContext
update-database -context ConfigDbContext
```
veritabanı nesneleri oluşturulabilir.

![image](https://user-images.githubusercontent.com/44789033/185818563-4be3e066-78db-4353-a98b-b0483c680273.png)
