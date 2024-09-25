
---

# Documentation de la sécurité mise en place

## 1. **Gestion sécurisée des mots de passe**

### a. Hachage des mots de passe
- Les mots de passe des utilisateurs sont hachés avant d'être stockés dans la base de données. Cela signifie que même en cas de fuite de données, les mots de passe ne sont pas stockés en texte clair, ce qui protège les utilisateurs contre le vol de leurs identifiants.
- Utilisation de **`PasswordHasher`** pour sécuriser les mots de passe avec un algorithme de hachage robuste.

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

- À chaque connexion, le mot de passe saisi par l'utilisateur est comparé avec le mot de passe haché stocké dans la base de données.

  Exemple de vérification :
  ```csharp
  var result = passwordHasher.VerifyHashedPassword(player, player.Password, loginRequest.Password);
  if (result == PasswordVerificationResult.Success)
  {
      // Connexion réussie
  }
  ```

### b. Validation de la complexité des mots de passe
- Les mots de passe doivent respecter des règles de complexité définies pour renforcer la sécurité (minimum 12 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial).
- Validation côté serveur dans la classe **`Player`** avec l'attribut **`[RegularExpression]`** et une méthode de validation personnalisée.

  Exemple de validation :
  ```csharp
  [Required]
  [StringLength(50, MinimumLength = 12, ErrorMessage = "Le mot de passe doit contenir entre 12 et 50 caractères.")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*]).+$", ErrorMessage = "Le mot de passe doit contenir une majuscule, une minuscule, un chiffre et un caractère spécial.")]
  public required string Password { get; set; }
  ```

## 2. **Validation des entrées utilisateur**

### a. Validation des pseudonymes des joueurs
- Les pseudonymes des joueurs sont soumis à des validations pour s'assurer qu'ils respectent certaines règles (longueur minimale et maximale, pas de caractères spéciaux).
  
  Exemple :
  ```csharp
  [Required]
  [StringLength(20, MinimumLength = 3, ErrorMessage = "Le nom doit avoir entre 3 et 20 caractères.")]
  [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Caractères spéciaux non autorisés.")]
  public required string Name { get; set; }
  ```

### b. Validation des données côté serveur
- Toute donnée envoyée par l'utilisateur est validée côté serveur pour s'assurer qu'elle est correcte et sécurisée avant d'être traitée ou stockée.
- La validation de la force du mot de passe, des pseudonymes et des entrées est effectuée avant d'enregistrer les données dans la base.

## 3. **Protection des communications avec HTTPS**

### a. Redirection automatique vers HTTPS
- Toutes les communications entre le client et le serveur sont protégées par **HTTPS**, garantissant que les données (comme les mots de passe) sont transmises de manière sécurisée.

  Code pour forcer la redirection HTTP vers HTTPS :
  ```csharp
  app.UseHttpsRedirection();
  ```

## 4. **Protection contre les scripts malveillants (XSS)**

### a. Échappement des caractères dangereux
- Les entrées utilisateur, comme les pseudonymes, sont nettoyées et échappées pour empêcher l'exécution de scripts malveillants dans l'interface utilisateur. Cela empêche les attaques de type **XSS** (Cross-Site Scripting).

  Exemple d'encodage HTML :
  ```csharp
  var encodedInput = System.Net.WebUtility.HtmlEncode(input);
  ```

### b. Validation des caractères autorisés
- Utilisation de **règles de validation** (comme l'attribut **`[RegularExpression]`**) pour restreindre les caractères autorisés dans les entrées utilisateur.

## 5. **Protection des API avec CORS**

### a. Utilisation de CORS pour protéger l'accès aux API
- **CORS** (Cross-Origin Resource Sharing) est configuré pour restreindre les appels d'API à des sources de confiance (comme ton front-end Vue.js). Cela protège contre les attaques **XSS** provenant d'autres domaines non autorisés.

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
- Utilisation de **`ILogger`** pour suivre et journaliser les événements critiques, tels que les connexions réussies, les tentatives échouées, et les modifications de données sensibles (comme les mots de passe).
  
  Exemple de journalisation :
  ```csharp
  _logger.LogInformation($"Tentative de connexion pour l'utilisateur : {username}");
  ```

## 7. **Gestion des sessions et cookies**

### a. Gestion sécurisée des sessions utilisateur
- Les sessions sont gérées de manière sécurisée avec une durée d'expiration configurée et des cookies marqués comme **HttpOnly** et **Secure** pour empêcher leur accès via du JavaScript ou des connexions non sécurisées.
  
  Exemple de gestion des sessions :
  ```csharp
  builder.Services.AddSession(options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de vie de la session
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
  });
  ```

## 8. **Mesures supplémentaires à envisager pour améliorer la sécurité**

### a. Protection contre les attaques CSRF (Cross-Site Request Forgery)
- Implémenter une protection **CSRF** pour empêcher les attaques de type **Cross-Site Request Forgery**. Cela garantit que les actions sensibles (comme la modification d'un profil ou l'envoi de formulaires) ne peuvent être effectuées que depuis le site légitime.

### b. Limitation des tentatives de connexion
- Limiter le nombre de tentatives de connexion échouées pour se protéger contre les attaques par **force brute**. Après plusieurs tentatives échouées, un utilisateur pourrait être temporairement bloqué ou soumis à un CAPTCHA.

### c. Authentification à deux facteurs (2FA)
- Pour renforcer la sécurité, implémenter l'authentification à deux facteurs (2FA), exigeant une validation supplémentaire (comme un code SMS ou une application de type Google Authenticator) lors de la connexion.

### d. Tests de pénétration
- Effectuer régulièrement des **tests de pénétration** pour identifier et corriger les failles de sécurité potentielles.

---