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

1. **Clonez le dépôt :**

   ```bash
   git clone https://github.com/Kylas07/cat-aclysme.git
   cd repo
   ```

2. **Naviguez vers le répertoire du backend :**

   ```bash
   cd back-end
   ```

3. **Restaurez les dépendances .NET :**

   ```bash
   dotnet restore
   ```

4. **Créez la base de données :**



5. **Appliquez les migrations Entity Framework Core :**

   ```bash
   dotnet ef database update
   ```

6. **Exécutez l'application backend :**

   ```bash
   dotnet run
   ```

### Frontend

1. **Naviguez vers le répertoire du frontend :**

   ```bash
   cd front-end
   ```

2. **Installez les dépendances Node.js :**

   ```bash
   npm install
   ```

4. **Installez Axios pour les appels API :**

   ```bash
   npm install axios
   ```

4. **Exécutez l'application frontend :**

   ```bash
   npm run serve
   ```

## Configuration

### Configuration du Backend

- Modifiez `appsettings.json` pour configurer la chaîne de connexion à la base de données :

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CatAclysmeDB;Trusted_Connection=True;"
  }
  ```

### Configuration du Frontend


## Utilisation

1. **Accéder à l'API Backend** :
   - Ouvrez un navigateur web ou un client API et aller à `http://localhost:5000/api/`.

2. **Accéder à l'application Frontend** :
   - Ouvrez un navigateur web et aller à `http://localhost:8080`.

