-- Forcer la fermeture des connexions à la base de données en mode multi-utilisateurs
USE master;
GO

-- Mettre la base de données en mode utilisateur unique pour déconnecter toutes les sessions
ALTER DATABASE CatAclysmeDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Supprimer la base de données
DROP DATABASE CatAclysmeDB;
GO
-- Création de la base de données
CREATE DATABASE CatAclysmeDB;
GO

-- Utilisation de la base de données
USE CatAclysmeDB;
GO

-- Création de la table Player
CREATE TABLE Player (
   playerId INT PRIMARY KEY IDENTITY(1,1),
   name VARCHAR(50) NOT NULL,
   password VARCHAR(255) NOT NULL
);

-- Création de la table Deck
CREATE TABLE Deck (
   deckId INT PRIMARY KEY IDENTITY(1,1),
   name VARCHAR(50) NOT NULL,
   playerId INT NOT NULL,
   FOREIGN KEY (playerId) REFERENCES Player(playerId)
);

-- Création de la table Card
CREATE TABLE Card (
   cardId INT PRIMARY KEY IDENTITY(1,1),
   name VARCHAR(255) NOT NULL,
   health INT NOT NULL,
   attack INT NOT NULL,
   image VARCHAR(255),
   description VARCHAR(255)
);

-- Création de la table Game
CREATE TABLE Game (
   gameId INT PRIMARY KEY IDENTITY(1,1),
   player1HP INT NOT NULL,
   player2HP INT NOT NULL,
   playerTurn INT NOT NULL,
   turnCount INT NOT NULL,
   gameStatus INT NOT NULL,
   playerId INT NOT NULL,
   playerId_1 INT NOT NULL,
   FOREIGN KEY (playerId) REFERENCES Player(playerId),
   FOREIGN KEY (playerId_1) REFERENCES Player(playerId)
);

-- Création de la table Score
CREATE TABLE Score (
   scoreId INT PRIMARY KEY IDENTITY(1,1),
   player1Score INT NOT NULL,
   player2Score INT NOT NULL,
   gameId INT NOT NULL,
   playerId INT NOT NULL,
   playerId_1 INT NOT NULL,
   FOREIGN KEY (gameId) REFERENCES Game(gameId),
   FOREIGN KEY (playerId) REFERENCES Player(playerId),
   FOREIGN KEY (playerId_1) REFERENCES Player(playerId)
);

-- Création de la table PlayerHand
CREATE TABLE PlayerHand (
   playerHandId INT PRIMARY KEY IDENTITY(1,1),
   positionInHand INT NOT NULL,
   cardAmount INT NOT NULL,
   gameId INT NOT NULL,
   playerId INT NOT NULL,
   cardId INT NOT NULL,
   FOREIGN KEY (gameId) REFERENCES Game(gameId),
   FOREIGN KEY (playerId) REFERENCES Player(playerId),
   FOREIGN KEY (cardId) REFERENCES Card(cardId)
);

-- Création de la table Turn
CREATE TABLE Turn (
   turnId INT PRIMARY KEY IDENTITY(1,1),
   gameId INT NOT NULL,
   cardId INT NOT NULL,
   cardId_1 INT NOT NULL,
   playerId INT NOT NULL,
   FOREIGN KEY (gameId) REFERENCES Game(gameId),
   FOREIGN KEY (cardId) REFERENCES Card(cardId),
   FOREIGN KEY (cardId_1) REFERENCES Card(cardId),
   FOREIGN KEY (playerId) REFERENCES Player(playerId)
);

-- Création de la table Build
CREATE TABLE Build (
   deckId INT NOT NULL,
   cardId INT NOT NULL,
   amount INT NOT NULL,
   PRIMARY KEY (deckId, cardId),
   FOREIGN KEY (deckId) REFERENCES Deck(deckId),
   FOREIGN KEY (cardId) REFERENCES Card(cardId)
);

-- Création de la table GameCard
CREATE TABLE GameCard (
   playerId INT NOT NULL,
   cardId INT NOT NULL,
   gameId INT NOT NULL,
   cardPosition INT NOT NULL,
   currentHealth INT NOT NULL,
   isActive BIT NOT NULL,
   PRIMARY KEY (playerId, cardId, gameId),
   FOREIGN KEY (playerId) REFERENCES Player(playerId),
   FOREIGN KEY (cardId) REFERENCES Card(cardId),
   FOREIGN KEY (gameId) REFERENCES Game(gameId)
);

-- Insertion de 50 cartes dans la table Card avec des noms basés sur des jeux de mots
INSERT INTO Card (name, health, attack, image, description) VALUES
('PasChat', 10, 5, 'paschat.png', 'Est-ce vraiment un chat ?'),
('En Cat-imini', 7, 8, 'en_catimini.png', 'Un chat furtif, qui se déplace en toute discrétion'),
('Chat-rlatan', 8, 7, 'chat_rlatan.png', 'Un chat trompeur qui manipule ses adversaires'),
('Chat-r-d’assaut', 15, 6, 'chat_r_d_assaut.png', 'Un puissant chat blindé, prêt à charger'),
('Chat-pristi', 9, 6, 'chapristi.png', 'Un chat qui surprend ses ennemis par sa vivacité'),
('Chat-teau', 12, 4, 'chat_teau.png', 'Un imposant chat qui règne sur son territoire comme un roi'),
('Chat-meau', 11, 5, 'chat_meau.png', 'Un chat robuste qui traverse les déserts sans effort'),
('Chat-an', 13, 6, 'chat_an.png', 'Un chat démoniaque dont le regard seul effraie'),
('Chat-hon', 8, 9, 'chat_hon.png', 'Un chat agile et rapide comme un poisson dans l’eau'),
('Chat-riot', 14, 5, 'cha_riot.png', 'Un chat transportant un lourd char, imparable sur le champ de bataille'),
('Chat-pardeur', 9, 8, 'cha_pardeur.png', 'Un chat voleur, expert dans l’art de la subtilisation'),
('Chat-rpie', 9, 7, 'chat_rpie.png', 'Un chat qui sème le désordre partout où il passe'),
('Chat-perlipopette', 10, 7, 'chat_perlipopette.png', 'Un chat qui surprend tout le monde avec ses coups inattendus'),
('Chat-ouille', 8, 9, 'chat_ouille.png', 'Un chat qui cause de la douleur avec des griffes acérées'),
('Chat-sseur', 11, 8, 'chat_sseur.png', 'Un chat redoutable chasseur, traquant sans relâche ses ennemis'),
('Chat-gnifique', 13, 5, 'chat_gnifique.png', 'Un chat d’une beauté et d’une force impressionnantes'),
('Chat-llenger', 10, 8, 'chat_llenger.png', 'Un chat qui défie les plus forts sans hésitation'),
('Chat-nibal', 14, 5, 'chat_nibal.png', 'Un chat sauvage, féroce et insatiable'),
('Chat-ctus', 11, 6, 'chat_ctus.png', 'Un chat épineux, dangereux à toucher'),
('Chat-pot', 12, 6, 'chat_pot.png', 'Un chat qui porte chance à ses alliés'),
('Chat-rlemagne', 14, 7, 'chat_rlemagne.png', 'Un chat impérial, conquérant des territoires avec bravoure'),
('Chat-rmand', 8, 8, 'chat_rmand.png', 'Un chat charmant, qui séduit ses adversaires par son élégance'),
('Chat-loupe', 9, 7, 'chat_loupe.png', 'Un chat naviguant sur les flots, manœuvrant habilement sa petite embarcation'),
('Chat-blier', 10, 6, 'chat_blier.png', 'Un chat qui contrôle le temps, faisant couler les secondes à sa guise'),
('Chat-ssassin', 8, 9, 'chat_ssassin.png', 'Un chat qui frappe dans l’ombre, silencieux et mortel'),
('Chat-lumeau', 10, 7, 'chat_lumeau.png', 'Un chat brillant qui éclaire les ténèbres avec sa lumière'),
('Chatt-pitre', 9, 6, 'chat_pitre.png', 'Un chat érudit, maîtrisant chaque chapitre de son savoir pour affaiblir ses adversaires'),
('Chat-otique', 10, 8, 'cha_otique.png', 'Un chat imprévisible, semant la confusion et le chaos sur le champ de bataille'),
('Chat-grin', 8, 7, 'cha_grin.png', 'Un chat mélancolique, dont la tristesse se transforme en force brute'),
('Chat-piteau', 8, 7, 'chat_piteau.png', 'Un chat clownesque sous le chapiteau, toujours prêt à surprendre ses ennemis avec ses farces');