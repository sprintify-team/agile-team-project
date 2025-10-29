# README – University Student Community Application

## 1️⃣ MSSQL
MSSQL Server localinizde kurulu olmalıdır.  
İsterseniz Docker üzerinden aşağıdaki komutla çalıştırabilirsiniz:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name mssql -d mcr.microsoft.com/mssql/server:2022-latest
```

`UniversityStudentCommunityApplicationBackend/WebApi/appsettings.json` dosyasındaki `Password` kısmına kendi SQL Server şifrenizi yazın:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SprintifyDB;User Id=sa;Password=<SIFRENIZ>;TrustServerCertificate=True;"
}
```

---

## 2️⃣ Backend (.NET)
```bash
cd UniversityStudentCommunityApplicationBackend/WebApi
dotnet restore
dotnet ef database update
dotnet run
```

API, **launchSettings.json** dosyanızda tanımlı portta (örneğin **https://localhost:7295**) çalışacaktır.

### 🔍 Test Etme (Swagger UI)
Uygulama çalıştığında Swagger otomatik olarak açılır:  
➡️ [https://localhost:7295/swagger/index.html](https://localhost:7295/swagger/index.html)

Swagger arayüzünden **GET /api/SystemMessage/{code}** endpointini kullanarak test edebilirsiniz.  
Örneğin:
```
GET /api/SystemMessage/COMING_SOON
```
isteği atıldığında aşağıdaki gibi bir yanıt döner:
```json
{
  "id": 1,
  "code": "COMING_SOON",
  "message": "Çok yakında hizmetinizdeyiz!"
}
```

---

## 3️⃣ Frontend (React + Vite)
```bash
cd university-student-community-application-frontend
npm install
npm run dev
```

React uygulaması varsayılan olarak **http://localhost:5173** portunda çalışır.  
Eğer bu port doluysa, Vite otomatik olarak bir sonraki boş portu (5174, 5175, ...) kullanır.

---

## 🔁 Çalıştırma Sırası
1. MSSQL Server’ı başlatın.  
2. Backend’i çalıştırın (`dotnet run`).  
3. Frontend’i çalıştırın (`npm run dev`).  
4. Tarayıcıdan frontend adresine gidin:  
   **http://localhost:5173**

---

## ℹ️ Notlar
- Portlar farklıysa frontend tarafında API çağrılarında kullanılan adresi güncelleyin.  
- Docker MSSQL konteynerini durdurup yeniden başlatmak için:
  ```bash
  docker start mssql
  ```  
- Veritabanı bağlantı sorunlarında `appsettings.json` dosyasındaki `Server`, `User Id` ve `Password` değerlerini kontrol edin.

---

Hepsi bu kadar ✅  
Sırasıyla **MSSQL → Backend → Frontend** adımlarını izleyerek projeyi kolayca çalıştırabilirsiniz.
