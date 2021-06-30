# .NET_TodoApi_Sqlite

  # ติดตั้ง dotnet-ef

```
dotnet tool install --global dotnet-ef
```

```
dotnet add package Microsoft.EntityFrameworkCore.Design
```

  # สร้าง mogrations
  
```
dotnet ef migrations add "Initial Migrations"
```

  # สร้าง database
  
```
dotnet ef database update
```

```
dotnet new gitignore
```

##jwt

```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI

```
---
dotnet ef migrations add "Adding authentication to out Api"
dotnet ef database update

# user-secrets

```
dotnet user-secrets init
```

```
dotnet user-secrets set JwtConfig:Secret 7HhdLiBBHTY9WavHr4Ggjus7EtW6vKVF
```
"JwtConfig":{"Secret": "7HhdLiBBHTY9WavHr4Ggjus7EtW6vKVF"}
