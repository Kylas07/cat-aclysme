# Projet de Jeu de Cartes

## Description



## Fonctionnalités

- **Gestion des Cartes**
- **Gestion des Joueurs**
- **Logique du Jeu**

## Stack Technologique

- **Backend** : .NET Core, Entity Framework Core, SQL Server Express
- **Frontend** : Vue.js
- **Base de Données** : SQL Server Express

## Prérequis

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 ou ultérieure)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Node.js et npm](https://nodejs.org/)
- [Vue CLI](https://cli.vuejs.org/) 

## Installation

### Backend

1. **Cloner le dépôt :**

   ```bash
   git clone https://github.com/tonutilisateur/ton-depot.git
   cd ton-depot
   ```

2. **Naviguer vers le répertoire du backend :**

   ```bash
   cd backend
   ```

3. **Restaurer les dépendances .NET :**

   ```bash
   dotnet restore
   ```

4. **Créer la base de données :**



5. **Appliquer les migrations Entity Framework Core :**

   ```bash
   dotnet ef database update
   ```

6. **Exécuter l'application backend :**

   ```bash
   dotnet run
   ```

### Frontend

1. **Naviguer vers le répertoire du frontend :**

   ```bash
   cd frontend
   ```

2. **Installer les dépendances Node.js :**

   ```bash
   npm install
   ```

3. **Exécuter l'application frontend :**

   ```bash
   npm run serve
   ```

## Configuration

### Configuration du Backend

- Modifier `appsettings.json` pour configurer la chaîne de connexion à la base de données :

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CardGameDB;Trusted_Connection=True;"
  }
  ```

### Configuration du Frontend

- Modifier `.env` ou les fichiers de configuration nécessaires pour les points de terminaison API ou autres paramètres.

## Utilisation

1. **Accéder à l'API Backend** :
   - Ouvre un navigateur web ou un client API et accède à `http://localhost:5000/api/`.

2. **Accéder à l'application Frontend** :
   - Ouvre un navigateur web et accède à `http://localhost:8080`.


N'hésite pas à adapter ce modèle en fonction des besoins spécifiques de ton projet et des détails que tu souhaites inclure.