
---

# Documentation de la sécurité mise en place

## 1. **Gestion sécurisée des mots de passe**

### a. Hachage des mots de passe
- Les mots de passe des utilisateurs sont **hachés avant d'être stockés** dans la base de données. Cela signifie que même en cas de fuite de données, les mots de passe ne sont pas stockés en texte clair.
- **`PasswordHasher`** est utilisé pour sécuriser les mots de passe avec un algorithme de hachage robuste.

  Exemple de code :
  ```csharp
  var passwordHasher = new PasswordHasher<Player>();
  var hashedPassword = passwordHasher.HashPassword(newPlayer, request.Password);

  var newPlayer = new Player
  {
      Name = request.PlayerName,
      Password = hashedPassword,
  };
  ```

- Lors de la connexion, le mot de passe saisi par l'utilisateur est comparé avec le mot de passe haché stocké dans la base de données.

  Exemple de vérification :
  ```csharp
  var result = passwordHasher.VerifyHashedPassword(player, player.Password, loginRequest.Password);
  if (result == PasswordVerificationResult.Success)
  {
      // Connexion réussie
  }
  ```

### b. Validation de la complexité des mots de passe
- Les mots de passe doivent respecter des **règles de complexité** : longueur minimale, majuscules, minuscules, chiffres, et caractères spéciaux.
  
  Exemple de validation côté serveur avec **`RegularExpression`** :
  ```csharp
  [Required]
  [StringLength(50, MinimumLength = 12, ErrorMessage = "Le mot de passe doit contenir entre 12 et 50 caractères.")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*]).+$", ErrorMessage = "Le mot de passe doit contenir une majuscule, une minuscule, un chiffre et un caractère spécial.")]
  public required string Password { get; set; }
  ```

## 2. **Validation des entrées utilisateur**

### a. Validation des pseudonymes des joueurs
- Les pseudonymes sont **validés** pour respecter certaines règles (longueur et exclusion des caractères spéciaux).
  
  Exemple :
  ```csharp
  [Required]
  [StringLength(20, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 20 caractères.")]
  [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Caractères spéciaux non autorisés.")]
  public required string Name { get; set; }
  ```

### b. Validation des données côté serveur
- Toutes les données envoyées sont validées côté serveur avant d'être traitées ou stockées.

## 3. **Protection des communications avec HTTPS**

### a. Redirection automatique vers HTTPS
- Toutes les communications sont protégées par **HTTPS**.
  
  Code pour forcer la redirection vers HTTPS :
  ```csharp
  app.UseHttpsRedirection();
  ```

## 4. **Protection contre les scripts malveillants (XSS)**

### a. Échappement des caractères dangereux
- Les entrées utilisateur, comme les pseudonymes, sont nettoyées pour empêcher l'exécution de scripts malveillants.

  Exemple d'encodage HTML :
  ```csharp
  var encodedInput = System.Net.WebUtility.HtmlEncode(input);
  ```

## 5. **Protection des API avec CORS**

### a. Utilisation de CORS pour protéger les API
- **CORS** (Cross-Origin Resource Sharing) est configuré pour restreindre l'accès aux API uniquement aux sources de confiance (comme ton front-end Vue.js).

  Exemple de configuration CORS :
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
  ```

## 6. **Journalisation et traçabilité**

### a. Journalisation des événements critiques
- Utilisation de **`ILogger`** pour journaliser les événements tels que les tentatives de connexion réussies ou échouées.

  Exemple de journalisation :
  ```csharp
  _logger.LogInformation($"Tentative de connexion pour l'utilisateur : {username}");
  ```

## 7. **Gestion des sessions et cookies**

### a. Gestion sécurisée des sessions
- Les sessions sont configurées avec une durée d'expiration et des cookies sécurisés **HttpOnly** et **Secure**.

  Exemple de gestion des sessions :
  ```csharp
  builder.Services.AddSession(options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de vie de la session
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
  });
  ```

## 8. **Journalisation dans un fichier avec Serilog**

### a. Utilisation de Serilog pour enregistrer les logs dans un fichier
- **Serilog** est utilisé pour journaliser dans la console et dans un fichier.
  
  Pour cela, tu dois installer les packages **`Serilog.AspNetCore`** et **`Serilog.Sinks.File`** :
  
  ```bash
  dotnet add package Serilog.AspNetCore
  dotnet add package Serilog.Sinks.File
  ```

  Ensuite, configure Serilog dans **`Program.cs`** :
  
  ```csharp
  using Serilog;

  var builder = WebApplication.CreateBuilder(args);

  Log.Logger = new LoggerConfiguration()
      .WriteTo.Console()
      .WriteTo.File("Logs/app-log-.txt", rollingInterval: RollingInterval.Day)
      .CreateLogger();

  builder.Host.UseSerilog();

  var app = builder.Build();
  ```

## 9. **Limitation des tentatives de connexion**

- Limiter les tentatives de connexion pour empêcher les attaques par **force brute**.

```csharp
using Microsoft.AspNetCore.Mvc;
using CatAclysmeApp.Data;
using CatAclysmeApp.Helpers;
using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;
using System.Threading.Tasks;
using System.Linq;
using back_end.Request;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory; // Pour l'utilisation de IMemoryCache
using System;

namespace CatAclysmeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly CatAclysmeContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache; // Utilisation de IMemoryCache pour stocker temporairement les tentatives échouées
        private readonly int _maxAttempts = 5; // Nombre maximum de tentatives avant blocage
        private readonly TimeSpan _lockoutDuration = TimeSpan.FromMinutes(15); // Durée du blocage après trop de tentatives échouées
    }

    ...

          // Vérifier si l'utilisateur a dépassé le nombre de tentatives autorisées
          if (_memoryCache.TryGetValue(cacheKey, out int attempts) && attempts >= _maxAttempts)
          {
          // Si l'utilisateur a dépassé le nombre de tentatives, il est temporairement bloqué
          _logger.LogWarning($"Trop de tentatives de connexion pour l'IP : {ipAddress}");
          return BadRequest($"Trop de tentatives de connexion. Veuillez réessayer après {_lockoutDuration.TotalMinutes} minutes.");
          }

    ...
}
```



## 10. **Mesures supplémentaires à envisager**

### a. Protection contre les attaques CSRF
- Implémenter des jetons **CSRF** pour protéger les actions sensibles.

### b. Tests de pénétration
- Effectuer des **tests de pénétration** pour identifier et corriger les vulnérabilités.

---