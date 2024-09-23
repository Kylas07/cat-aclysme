### Aide pour le back-end

#### 1. **Installer les outils nécessaires**

- **.NET SDK** : Télécharge et installe le SDK .NET 6 ou plus récent depuis [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

- **SQL Server** ou **SQL Server Express** : Pour la base de données. Si ce n'est pas installé, télécharge-le [ici](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads).

- **Visual Studio** ou **Visual Studio Code** : Pour écrire et gérer le code (Visual Studio a de meilleures intégrations pour les projets ASP.NET Core).

#### 2. **Créer un nouveau projet ASP.NET Core**

Commence par créer un nouveau projet **ASP.NET Core Web API** :

```bash
dotnet new webapi -n CatAclysmeApp
cd CatAclysmeApp
```

Cela crée la structure de base de ton projet avec un modèle d'API.

#### 3. **Installer les dépendances nécessaires**

Ensuite, installe les packages nécessaires pour gérer l’accès à la base de données et la sécurité.

1. **Installer Entity Framework Core** pour la gestion de la base de données SQL Server :

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

2. **Installer Swagger** pour documenter l'API :

```bash
dotnet add package Swashbuckle.AspNetCore
```

#### 4. **Configurer la connexion à la base de données**

Dans le fichier **`appsettings.json`**, ajoute ta chaîne de connexion à la base de données SQL Server :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=CatAclysmeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Ensuite, dans **`Program.cs`**, configure l'accès à la base de données :

```csharp
builder.Services.AddDbContext<CatAclysmeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

#### 5. **Créer le modèle de base de données (entités)**

Crée une structure pour les entités de ton application, comme les joueurs (`Player`) et les parties (`Game`). Ces entités seront utilisées pour mapper les données de la base de données.

Dans le dossier **`Models`**, crée des fichiers comme **Player.cs** et **Game.cs**.

**Exemple pour `Player.cs`** :

```csharp
public class Player
{
    public int PlayerId { get; set; }
    public string Name { get; set; }
}
```

**Exemple pour `Game.cs`** :

```csharp
public class Game
{
    public int GameId { get; set; }
    public int Player1HP { get; set; }
    public int Player2HP { get; set; }
    public int PlayerTurn { get; set; }
    public int TurnCount { get; set; }

    public int PlayerId { get; set; }
    public int PlayerId_1 { get; set; }
    public Player Player { get; set; }
    public Player Player_1 { get; set; }
}
```

#### 6. **Configurer le `DbContext`**

Crée une classe **CatAclysmeContext** dans le dossier **`Data`** pour gérer l'accès à la base de données avec **Entity Framework Core**.

```csharp
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;

public class CatAclysmeContext : DbContext
{
    public CatAclysmeContext(DbContextOptions<CatAclysmeContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().ToTable("Player");
        modelBuilder.Entity<Game>().ToTable("Game");
    }
}
```

#### 7. **Migrations et base de données**

1. Crée la migration initiale pour générer la base de données à partir des modèles :

```bash
dotnet ef migrations add InitialCreate
```

2. Applique cette migration pour créer les tables dans ta base de données SQL Server :

```bash
dotnet ef database update
```

#### 8. **Créer les contrôleurs d'API**

Maintenant, crée les contrôleurs pour exposer les endpoints API.

1. Dans le dossier **`Controllers`**, crée un fichier **GameController.cs** qui gérera la création des parties et la gestion du jeu :

```csharp
using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly CatAclysmeContext _context;

    public GameController(CatAclysmeContext context)
    {
        _context = context;
    }

    // POST : api/game/start
    [HttpPost("start")]
    public async Task<IActionResult> StartGame([FromBody] GameStartRequest request)
    {
        if (string.IsNullOrEmpty(request.Player1Pseudo) || string.IsNullOrEmpty(request.Player2Pseudo))
        {
            return BadRequest(new { message = "Les pseudos des joueurs sont requis." });
        }

        var player1 = _context.Players.SingleOrDefault(p => p.Name == request.Player1Pseudo) ?? new Player { Name = request.Player1Pseudo };
        var player2 = _context.Players.SingleOrDefault(p => p.Name == request.Player2Pseudo) ?? new Player { Name = request.Player2Pseudo };

        if (player1.PlayerId == 0) _context.Players.Add(player1);
        if (player2.PlayerId == 0) _context.Players.Add(player2);

        await _context.SaveChangesAsync();

        var game = new Game
        {
            Player1HP = 100,
            Player2HP = 100,
            PlayerTurn = player1.PlayerId,
            TurnCount = 1,
            Player = player1,
            Player_1 = player2
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return Ok(new { gameId = game.GameId });
    }
}

public class GameStartRequest
{
    public string Player1Pseudo { get; set; }
    public string Player2Pseudo { get; set; }
}
```

#### 9. **Configurer CORS**

Ajoute une configuration CORS dans **`Program.cs`** pour permettre à ton frontend Vue.js de faire des appels à l'API :

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

app.UseCors("AllowVueApp");
```

#### 10. **Configurer Swagger**

Pour documenter ton API et tester facilement les endpoints, active **Swagger** dans **`Program.cs`** :

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatAclysmeApp API V1");
    });
}
```

#### 11. **Exécuter l'application**

Lance ton application en mode développement :

```bash
dotnet run
```

L'API sera disponible à l'adresse **https://localhost:7111** (ou un autre port, selon la configuration). Tu peux tester les endpoints API dans Swagger à l'URL `https://localhost:7111/swagger`.

#### 12. **Tester l'API avec Vue.js**

Le frontend Vue.js enverra des requêtes **POST** à ton API, par exemple à l'endpoint `https://localhost:7111/api/game/start` pour démarrer une partie.