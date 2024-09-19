
---

# Tutoriel de Configuration pour le Projet **CatAclysme**

## Prérequis

1. **.NET Core SDK** : Assurez-vous d'avoir installé le .NET Core SDK sur votre machine.
   - Téléchargement : [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. **SQL Server Express** : Vous aurez besoin de SQL Server Express pour héberger la base de données.
   - Téléchargement : [https://www.microsoft.com/en-us/sql-server/sql-server-downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

3. **Entity Framework Core CLI** (si ce n'est pas déjà installé) :
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Node.js** (pour le front-end avec Vue.js)
   - Téléchargement : [https://nodejs.org/](https://nodejs.org/)

## Structure du Projet

Votre projet devrait avoir la structure suivante :

```
cat-aclysme/
├── back-end/
│   ├── back-end.csproj
│   ├── Controllers
│   │     └──HomeController.cs
│   ├── Views
│   │     └──Home
│   │         └──Index.cshtml
│   ├── Data
│   │     └──CatAclysmeContext.cs
│   ├── Models
│   │     └── Player.cs
│   ├── Program.cs
│   ├── Startup.cs
│   └── Properties/launchSettings.json
```

## Étapes d'Installation et de Configuration

### 1. Création du Projet **ASP.NET Core MVC**

Commencez par créer un nouveau projet **ASP.NET Core MVC** dans le dossier `back-end` :

```bash
dotnet new mvc -n back-end
```

### 2. Installer les dépendances nécessaires

- **Entity Framework Core** (pour SQL Server) :
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   ```

- **Swagger** (pour la documentation API) :
   ```bash
   dotnet add package Swashbuckle.AspNetCore
   ```

### 3. Configuration de la base de données SQL Server

#### A. Chaîne de connexion

Ajoutez la chaîne de connexion dans **`appsettings.json`** :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CatAclysmeDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "AllowedHosts": "*"
}
```

#### B. Création du contexte de base de données

Créez un fichier **`Data/CatAclysmeContext.cs`** pour définir le contexte de base de données :

```csharp
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;

namespace CatAclysmeApp.Data
{
    public class CatAclysmeContext : DbContext
    {
        public CatAclysmeContext(DbContextOptions<CatAclysmeContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().ToTable("Player");  // Nom exact de la table
            modelBuilder.Entity<Card>().ToTable("Card");
        }
    }
}
```

#### C. Configuration de `Program.cs`

Dans **`Program.cs`**, ajoutez la configuration du service **Entity Framework** :

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatAclysmeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 4. Création des Modèles

Créez un modèle pour `Player` et `Card` dans **`Models/Player.cs`** et **`Models/Card.cs`**.

**Exemple pour Player** :
```csharp
namespace CatAclysmeApp.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
```

### 5. Test de la Connexion à la Base de Données

Dans **`Controllers/HomeController.cs`**, ajoutez une action pour tester l'accès à la base de données :

```csharp
public IActionResult TestDb()
{
    var playerCount = _context.Players.Count();
    return Content($"Nombre de joueurs dans la base de données : {playerCount}");
}
```

Accédez à l'URL suivante pour tester :
```
https://localhost:7111/home/testdb
```

### 6. Désactiver les Fichiers Statiques

Si vous n'utilisez pas de fichiers statiques dans le back-end, commentez la ligne suivante dans **`Program.cs`** :

```csharp
// app.UseStaticFiles();
```

### 7. Configuration HTTPS

Si vous rencontrez des problèmes avec HTTPS, vous pouvez désactiver temporairement **`HttpsRedirection`** dans **`Program.cs`** ou ajuster le certificat via **TrustServerCertificate=True** dans votre chaîne de connexion.

### 10. Résolution des erreurs de version (`Microsoft.Data.SqlClient`)

Si vous voyez des erreurs concernant une vulnérabilité ou des incompatibilités de version pour **`Microsoft.Data.SqlClient`**, assurez-vous d'utiliser la version 5.1.5 ou supérieure :

```bash
dotnet add package Microsoft.Data.SqlClient --version 5.1.5
```

---