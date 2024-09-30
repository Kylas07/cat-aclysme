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
   password NVARCHAR(255) NOT NULL
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
('PasChat', 10, 5, 'paschat.webp', 'Est-ce vraiment un chat ?'),
('En Cat-imini', 7, 8, 'en_catimini.webp', 'Un chat furtif, qui se déplace en toute discrétion'),
('Chat-rlatan', 8, 7, 'chat_rlatan.webp', 'Un chat trompeur qui manipule ses adversaires'),
('Chat-r-d’assaut', 15, 6, 'chat_r_d_assaut.webp', 'Un puissant chat blindé, prêt à charger'),
('Chat-pristi', 9, 6, 'chapristi.webp', 'Un chat qui surprend ses ennemis par sa vivacité'),
('Chat-teau', 12, 4, 'chat_teau.webp', 'Un imposant chat qui règne sur son territoire comme un roi'),
('Chat-meau', 11, 5, 'chat_meau.webp', 'Un chat robuste qui traverse les déserts sans effort'),
('Chat-an', 13, 6, 'chat_an.webp', 'Un chat démoniaque dont le regard seul effraie'),
('Chat-hon', 8, 9, 'chat_hon.webp', 'Un chat agile et rapide comme un poisson dans l’eau'),
('Chat-riot', 14, 5, 'cha_riot.webp', 'Un chat transportant un lourd char, imparable sur le champ de bataille'),
('Chat-pardeur', 9, 8, 'cha_pardeur.webp', 'Un chat voleur, expert dans l’art de la subtilisation'),
('Chat-rpie', 9, 7, 'chat_rpie.webp', 'Un chat qui sème le désordre partout où il passe'),
('Chat-perlipopette', 10, 7, 'chat_perlipopette.webp', 'Un chat qui surprend tout le monde avec ses coups inattendus'),
('Chat-ouille', 8, 9, 'chat_ouille.webp', 'Un chat qui cause de la douleur avec des griffes acérées'),
('Chat-sseur', 11, 8, 'chat_sseur.webp', 'Un chat redoutable chasseur, traquant sans relâche ses ennemis'),
('Chat-gnifique', 13, 5, 'chat_gnifique.webp', 'Un chat d’une beauté et d’une force impressionnantes'),
('Chat-llenger', 10, 8, 'chat_llenger.webp', 'Un chat qui défie les plus forts sans hésitation'),
('Chat-nibal', 14, 5, 'chat_nibal.webp', 'Un chat sauvage, féroce et insatiable'),
('Chat-ctus', 11, 6, 'chat_ctus.webp', 'Un chat épineux, dangereux à toucher'),
('A-Cat-47', 12, 6, 'a_cat_47.webp', 'Un chat fou furieux bien armé !'),
('Chat-rlemagne', 14, 7, 'chat_rlemagne.webp', 'Un chat impérial, conquérant des territoires avec bravoure'),
('Chat-rmant', 8, 8, 'chat_rmant.webp', 'Un chat charmant, qui séduit ses adversaires par son élégance'),
('Chat-loupe', 9, 7, 'chat_loupe.webp', 'Un chat naviguant sur les flots, manœuvrant habilement sa petite embarcation'),
('Chat-blier', 10, 6, 'chat_blier.webp', 'Un chat qui contrôle le temps, faisant couler les secondes à sa guise'),
('Chat-ssassin', 8, 9, 'chat_ssassin.webp', 'Un chat qui frappe dans l’ombre, silencieux et mortel'),
('Chat-lumeau', 10, 7, 'chat_lumeau.webp', 'Un chat brillant qui éclaire les ténèbres avec sa lumière'),
('Chat-pitre', 9, 6, 'chat_pitre.webp', 'Un chat érudit, maîtrisant chaque chapitre de son savoir pour affaiblir ses adversaires'),
('Chat-otique', 10, 8, 'cha_otique.webp', 'Un chat imprévisible, semant la confusion et le chaos sur le champ de bataille'),
('Chat-grin', 8, 7, 'cha_grin.webp', 'Un chat mélancolique, dont la tristesse se transforme en force brute'),
('Chat-piteau', 8, 7, 'chat_piteau.webp', 'Un chat clownesque sous le chapiteau, toujours prêt à surprendre ses ennemis avec ses farces');


-- Insertion des images dans la table Card /!\chemin à modifier 
INSERT INTO Card (name, health, attack, image, description) 
VALUES
('PasChat', 10, 5, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\paschat.webp', SINGLE_BLOB) AS Image), 'Est-ce vraiment un chat ?');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('En Cat-imini', 7, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\en_catimini.webp', SINGLE_BLOB) AS Image), 'Un chat furtif, qui se déplace en toute discrétion');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-rlatan', 8, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_rlatan.webp', SINGLE_BLOB) AS Image), 'Un chat trompeur qui manipule ses adversaires');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-r-d’assaut', 15, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_r_d_assaut.webp', SINGLE_BLOB) AS Image), 'Un puissant chat blindé, prêt à charger');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-pristi', 9, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_pristi.webp', SINGLE_BLOB) AS Image), 'Un chat qui surprend ses ennemis par sa vivacité');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-teau', 12, 4, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_teau.webp', SINGLE_BLOB) AS Image), 'Un imposant chat qui règne sur son territoire comme un roi');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-meau', 11, 5, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_meau.webp', SINGLE_BLOB) AS Image), 'Un chat robuste qui traverse les déserts sans effort');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-an', 13, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_an.webp', SINGLE_BLOB) AS Image), 'Un chat démoniaque dont le regard seul effraie');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-hon', 8, 9, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_hon.webp', SINGLE_BLOB) AS Image), 'Un chat agile et rapide comme un poisson dans l’eau');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-riot', 14, 5, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_riot.webp', SINGLE_BLOB) AS Image), 'Un chat transportant un lourd char, imparable sur le champ de bataille');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-pardeur', 9, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_pardeur.webp', SINGLE_BLOB) AS Image), 'Un chat voleur, expert dans l’art de la subtilisation');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-rpie', 9, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_rpie.webp', SINGLE_BLOB) AS Image), 'Un chat qui sème le désordre partout où il passe');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-perlipopette', 10, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_perlipopette.webp', SINGLE_BLOB) AS Image), 'Un chat qui surprend tout le monde avec ses coups inattendus');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-ouille', 8, 9, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_ouille.webp', SINGLE_BLOB) AS Image), 'Un chat qui cause de la douleur avec des griffes acérées');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-sseur', 11, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_sseur.webp', SINGLE_BLOB) AS Image), 'Un chat redoutable chasseur, traquant sans relâche ses ennemis');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-gnifique', 13, 5, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_gnifique.webp', SINGLE_BLOB) AS Image), 'Un chat d’une beauté et d’une force impressionnantes');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-llenger', 10, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_llenger.webp', SINGLE_BLOB) AS Image), 'Un chat qui défie les plus forts sans hésitation');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-nibal', 14, 5, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_nibal.webp', SINGLE_BLOB) AS Image), 'Un chat sauvage, féroce et insatiable');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-ctus', 11, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_ctus.webp', SINGLE_BLOB) AS Image), 'Un chat épineux, dangereux à toucher');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('A-Cat-47', 12, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\a_cat_47.webp', SINGLE_BLOB) AS Image), 'Un chat fou furieux bien armé !');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-rlemagne', 14, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_rlemagne.webp', SINGLE_BLOB) AS Image), 'Un chat impérial, conquérant des territoires avec bravoure');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-rmant', 8, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_rmant.webp', SINGLE_BLOB) AS Image), 'Un chat charmant, qui séduit ses adversaires par son élégance');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-loupe', 9, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_loupe.webp', SINGLE_BLOB) AS Image), 'Un chat naviguant sur les flots, manœuvrant habilement sa petite embarcation');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-blier', 10, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_blier.webp', SINGLE_BLOB) AS Image), 'Un chat qui contrôle le temps, faisant couler les secondes à sa guise');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-ssassin', 8, 9, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_ssassin.webp', SINGLE_BLOB) AS Image), 'Un chat qui frappe dans l’ombre, silencieux et mortel');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-lumeau', 10, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_lumeau.webp', SINGLE_BLOB) AS Image), 'Un chat brillant qui éclaire les ténèbres avec sa lumière');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-pitre', 9, 6, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_pitre.webp', SINGLE_BLOB) AS Image), 'Un chat érudit, maîtrisant chaque chapitre de son savoir pour affaiblir ses adversaires');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-otique', 10, 8, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_otique.webp', SINGLE_BLOB) AS Image), 'Un chat imprévisible, semant la confusion et le chaos sur le champ de bataille');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-grin', 8, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_grin.webp', SINGLE_BLOB) AS Image), 'Un chat mélancolique, dont la tristesse se transforme en force brute');

INSERT INTO Card (name, health, attack, image, description) 
VALUES
('Chat-piteau', 8, 7, (SELECT * FROM OPENROWSET(BULK N'C:\Users\ghils\Desktop\Projet\Projet_groupe\Cat-Aclysme\cat-aclysme\front-end\src\assets\img\chat_piteau.webp', SINGLE_BLOB) AS Image), 'Un chat clownesque sous le chapiteau, toujours prêt à surprendre ses ennemis avec ses farces');
