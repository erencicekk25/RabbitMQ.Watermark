# RabbitMQ.Watermark


🙌 Merhabalar, ASP.NET Core MVC ile geliştirmiş olduğum bu proje RabbitMQ'u daha iyi anlamak için yaptım. Projedeki asıl amaç girilen bir fotoğrafın üzerine "www.mysite.com" yazmak ve bunu yapabilmek için 🐰 RabbitMQ üzerinden Exchange ve queue'dan yardım aldım.

Projemde kullanmış olduğum bazı .NET kütüphaneleri;
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.EntityFrameworkCore.Tools
- RabbitMQ.Client
- System.Drawing.Common


## Çalıştırma

`appsettings.json` içindeki bağlantı adresi yerel bir RabbitMQ sunucusunu (`amqp://guest:guest@localhost:5672`) gösteriyor. Gerçek bir sunucunun adresini dosyaya yazmak yerine ortam değişkeni ya da user-secrets ile verin:

```bash
# Ortam değişkeni (ASP.NET Core bunu ConnectionStrings:RabbitMQ olarak okur)
export ConnectionStrings__RabbitMQ="amqps://kullanici:parola@sunucu/vhost"

# ya da user-secrets
dotnet user-secrets set "ConnectionStrings:RabbitMQ" "amqps://kullanici:parola@sunucu/vhost" --project RabbitMQWeb.Watermark
```

Yerelde denemek için: `docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management`
