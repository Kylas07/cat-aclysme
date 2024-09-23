# Sécurité

#### 1. **Validation des saisies utilisateur (côté client et serveur)**
   
La validation des saisies utilisateur est une mesure essentielle pour prévenir les attaques de type **injection** ou **XSS** (Cross-Site Scripting). Voici les mesures mises en place :
   
- **Côté client (Vue.js)** :
  - Validation des champs du formulaire avant envoi. Par exemple, les champs de pseudos pour les joueurs sont contrôlés en termes de longueur (minimum et maximum).
  - Utilisation de composants de formulaires pour assurer que les données soient bien formatées avant d’être envoyées au serveur.

- **Côté serveur (ASP.NET Core)** :
  - Utilisation de **Data Annotations** (ou validation personnalisée) pour contrôler les données reçues par l'API. Par exemple, la classe `GameStartRequest` vérifie que les pseudos ne sont pas vides et qu'ils respectent les règles de validation (longueur maximale, contenu autorisé).
  - Vérification que les données envoyées sont bien formatées et sécurisées via l'attribut `[FromBody]` dans les méthodes POST du contrôleur.
   
#### 2. **Contraintes au niveau de la base de données**
   
La base de données est sécurisée en utilisant des **contraintes** pour s'assurer de l'intégrité des données :
   
- **Intégrité référentielle** : Les relations entre les tables, telles que les joueurs et les parties, sont maintenues via des **clés étrangères**. Par exemple, une partie ne peut pas exister sans un joueur.
- **Contrainte de non-nullabilité** : Certains champs comme les noms des joueurs sont définis comme **non null** dans la base de données pour éviter les valeurs manquantes ou incorrectes.
- **Utilisation des clés composites** : Pour les entités comme `Build` et `GameCard`, des **clés composites** sont utilisées afin de garantir que les enregistrements dans ces tables soient uniques et que les relations entre les entités soient correctement établies.

#### 3. **Configurations au niveau du serveur**
   
Les configurations suivantes ont été mises en place pour assurer la sécurité au niveau du serveur :

- **HTTPS** : L'application utilise **HTTPS** (SSL/TLS) pour chiffrer les données lors de leur transfert entre le client (Vue.js) et le serveur (ASP.NET Core).
- **CORS** : Une **politique CORS** (Cross-Origin Resource Sharing) est configurée pour restreindre les requêtes aux seuls domaines autorisés, en l’occurrence `http://localhost:8080` pour le frontend Vue.js. Cela prévient les attaques Cross-Site Request Forgery (CSRF).
- **Limitation des méthodes HTTP** : Seules les méthodes HTTP nécessaires (GET, POST, etc.) sont autorisées via le backend pour éviter l'exposition inutile des autres méthodes.

#### 4. **Utilisation de transactions**
   
- **Transactions** : Lors des opérations critiques, comme la création d'une partie ou l'ajout de joueurs, des **transactions** sont utilisées dans la base de données pour garantir que toutes les opérations sont complétées avec succès ou qu'aucune modification n'est effectuée en cas d'erreur.
- Par exemple, lors de la création d'une partie, si la création d'un joueur échoue, toutes les opérations précédentes dans cette transaction sont annulées (rollback), ce qui garantit l'intégrité des données.

#### 5. **OWASP TOP 10**
   
Plusieurs mesures sont prises pour éviter les vulnérabilités courantes mentionnées dans le **OWASP Top 10** :

- **Injection** : Protection contre les injections SQL avec **Entity Framework Core**, qui utilise des requêtes paramétrées et évite la concaténation directe de chaînes SQL.
- **Cross-Site Scripting (XSS)** : Filtrage et validation des données côté client et serveur pour empêcher l'exécution de balises HTML ou de scripts malveillants.
- **Cross-Site Request Forgery (CSRF)** : L'API ne permet les requêtes que depuis les origines autorisées (via CORS) et prévoit des mécanismes de protection contre les requêtes non autorisées.

#### 6. **Journalisation / Traçabilité**
   
Une journalisation appropriée a été mise en place pour aider à identifier et retracer les actions et événements sur le serveur :

- **Journalisation des actions utilisateurs** : Les actions critiques comme la création de parties ou les erreurs rencontrées sont journalisées avec les détails pertinents (par exemple, l’ID du jeu ou du joueur).
- **Traçabilité des erreurs** : Les erreurs côté serveur sont loggées avec des messages détaillés à l'aide du système de journalisation de **Microsoft.Extensions.Logging**.

#### 7. **Tests de pénétration**
   
Bien que non encore réalisés, des **tests de pénétration** sont recommandés pour identifier d'éventuelles failles de sécurité. Ces tests incluraient :
   
- **Tests d'injection** : Tester si les requêtes utilisateur peuvent déclencher une injection SQL.
- **Tests XSS** : S'assurer que les champs de saisie utilisateur sont bien protégés contre l'injection de scripts malveillants.
- **Tests CSRF** : Vérifier que les mécanismes de protection CSRF fonctionnent correctement.

#### 8. **Résilience : reprise après crash serveur**
   
L'application est conçue pour être **résiliente** en cas de panne ou de crash du serveur :

- **Transactions atomiques** : L'utilisation de transactions atomiques garantit que les opérations en cours sont soit terminées avec succès, soit annulées en cas d'échec.
- **Gestion des erreurs** : Une gestion centralisée des erreurs est mise en place pour renvoyer des réponses d'erreur uniformes en cas de problème côté serveur, ce qui assure une meilleure expérience utilisateur et aide au diagnostic.

---

### Éléments de sécurité supplémentaires à envisager :

1. **Authentification / Autorisation** :
   - Si l'application évolue pour gérer des utilisateurs connectés, l'ajout d'un système d'authentification basé sur des tokens (comme **JWT**) est recommandé.
   
2. **Limitation des taux de requêtes (Rate Limiting)** :
   - Limiter le nombre de requêtes par utilisateur ou IP pour éviter les attaques par déni de service (DoS).

3. **Protection contre les attaques par force brute** :
   - Si un jour l'application inclut des connexions utilisateurs, il est important d'ajouter une protection contre les attaques par force brute, comme le verrouillage du compte après plusieurs tentatives de connexion échouées.

---

### Conclusion

Ces mesures de sécurité ont été mises en place pour protéger l'intégrité des données, assurer la résilience du système et prévenir les vulnérabilités courantes. En complément, des mesures supplémentaires telles que l'authentification et les tests de pénétration sont à envisager pour améliorer encore la sécurité de l'application.