# SportEventApp

# 🏟️ SportsEventApp – Orleans Based Sports Event Management System

## 📌 Proje Hakkında

Bu proje, spor etkinlikleri için bir kayıt sistemi sunar. Katılımcılar etkinliklere kayıt olabilir, kayıtlı kullanıcıları listeleyebilir. Sistem Orleans grain mimarisi üzerine kuruludur, veriler hem memory'de (persistant grain state) hem de MSSQL veritabanında tutulur. Web API aracılığıyla tüm işlemler erişilebilir.

---

## 🛠️ Kullanılan Teknolojiler

- [.NET 8](https://dotnet.microsoft.com/)
- [Orleans](https://learn.microsoft.com/en-us/dotnet/orleans/)
- MSSQL (stored procedure bazlı erişim)
- Dapper (ORM kullanılmadan)
- Swagger (API dokümantasyonu)
- Health Checks (MSSQL + Orleans kontrolü)

---

## 🧱 Katmanlar

| Katman | Açıklama |
|--------|----------|
| **API** | RESTful endpoint'ler sağlar |
| **Grains** | Orleans grain logic'leri içerir (stateful) |
| **GrainInterfaces** | Orleans arayüzlerini içerir |
| **Infrastructure** | Dapper tabanlı MSSQL erişimi ve SP çağrıları |
| **Silo** | Grainlerin ayaklandığı yapıyı barındırır |

---

## 🧪 Endpoint'ler

- `GET /api/sportevents` → Tüm etkinlikleri listeler
- `POST /api/sportevents/{id}/register` → Kullanıcıyı belirtilen etkinliğe kayıt eder
- `GET /api/sportevents/{id}/participants` → Etkinliğe kayıtlı kullanıcıları listeler
- `GET /health` → MSSQL + Orleans cluster kontrolü

---

## 📦 Persistent State Yapısı

```csharp
public class SportEventState
{
    public SportEventDto? EventInfo { get; set; }
    public List<string> RegisteredUsers { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsUsersLoaded { get; set; } = false;
}
```

## 🛠 Veritabanı Kurulumu (init.sql)
Proje ile birlikte gelen init.sql dosyası sayesinde, MSSQL veritabanı tek seferde otomatik olarak başlatılabilir. Bu dosya aşağıdaki işlemleri içerir:

Gerekli tabloların oluşturulması (SportEvents, SportEventRegistrations)

İlgili stored procedure'lerin oluşturulması

Örnek etkinlik verilerinin eklenmesi

📦 Kullanım
init.sql dosyasını SQL Server Management Studio, Azure Data Studio ya da CLI üzerinden açın.

Hedef veritabanını seçerek çalıştırın.

Uygulama tarafında konfigürasyonda tanımlı connection string ile sistem otomatik olarak bu verilerle entegre çalışacaktır.
