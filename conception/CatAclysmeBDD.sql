-- Drop database if it exists and create a new one
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'CatAclysmeDB')
BEGIN
    DROP DATABASE CatAclysmeDB;
END;
GO

CREATE DATABASE CatAclysmeDB;
GO

USE CatAclysmeDB;
GO

-- Table Player
CREATE TABLE Player (
    player_id INT PRIMARY KEY IDENTITY(1,1),
    player_name VARCHAR(50),
    player_hp INT,
    player_turn BIT
);

-- Table Deck
CREATE TABLE Deck (
    deck_id INT PRIMARY KEY IDENTITY(1,1),
    player_id INT,
    FOREIGN KEY (player_id) REFERENCES Player(player_id)
);

-- Table Cards
CREATE TABLE Cards (
    card_id INT PRIMARY KEY IDENTITY(1,1),
    card_name VARCHAR(50),
    card_hp INT,
    card_atk INT,
    card_illustration TEXT,
    card_description TEXT 
);

-- Table Game
CREATE TABLE Game (
    game_id INT PRIMARY KEY IDENTITY(1,1),
    player1_id INT,
    player2_id INT,
    FOREIGN KEY (player1_id) REFERENCES Player(player_id),
    FOREIGN KEY (player2_id) REFERENCES Player(player_id)
);

-- Table Score
CREATE TABLE Score (
    score_id INT PRIMARY KEY IDENTITY(1,1),
    game_id INT,
    player1_score INT,
    player2_score INT,
    FOREIGN KEY (game_id) REFERENCES Game(game_id)
);

-- Table associating Deck and Cards (Deck_Cards)
CREATE TABLE Deck_Cards (
    deck_id INT,
    card_id INT,
    PRIMARY KEY (deck_id, card_id),
    FOREIGN KEY (deck_id) REFERENCES Deck(deck_id),
    FOREIGN KEY (card_id) REFERENCES Cards(card_id)
);
GO


-- Insertion des 50 cartes
INSERT INTO Cards (card_name, card_hp, card_atk, card_illustration, card_description)
VALUES
(N'Chratgnar', 70, 25, N'images/chratgnar.png', N'Un chat viking avec un casque et une hache, inspiré de Ragnar.'),
(N'Chatja le Ninja', 40, 50, N'images/chatja_ninja.png', N'Un chat agile avec des étoiles de ninja, expert en furtivité.'),
(N'Chat Sparrow', 60, 30, N'images/chat_sparrow.png', N'Un chat pirate avec un sabre, inspiré par Jack Sparrow.'),
(N'Chatmorak', 100, 10, N'images/chatmorak.png', N'Un chat tank massif, difficile à abattre, comme un mur.'),
(N'Chatron', 50, 40, N'images/chatron.png', N'Un chat sorcier maîtrisant la magie, inspiré de Merlin.'),
(N'Chatman', 30, 60, N'images/chatman.png', N'Un chat assassin avec une cape noire, inspiré par Batman.'),
(N'Chatoscope', 30, 40, N'images/chatoscope.png', N'Un chat sniper avec une précision redoutable.'),
(N'Chatfufu', 35, 55, N'images/chatfufu.png', N'Un chat ninja qui se fond dans l''ombre.'),
(N'Chatalienne', 80, 20, N'images/chatalienne.png', N'Un chat barbare avec une masse énorme, inspiré de Conan.'),
(N'Chalchemy', 40, 35, N'images/chalchemy.png', N'Un chat alchimiste qui prépare des potions.'),
(N'Chatvalier', 90, 15, N'images/chatvalier.png', N'Un chat en armure brillante avec une épée.'),
(N'Chatarreau', 55, 45, N'images/chatarreau.png', N'Un chat artilleur expert en armes lourdes.'),
(N'Chaboteur', 35, 50, N'images/chaboteur.png', N'Un chat saboteur expert en explosifs.'),
(N'Chatgnole', 50, 30, N'images/chatgnole.png', N'Un chat avec deux couteaux acérés.'),
(N'Chatazelle', 40, 40, N'images/chatazelle.png', N'Un chat avec un jetpack, maître du combat aérien.'),
(N'Chatick', 35, 55, N'images/chatick.png', N'Un chat tireur d''élite à la précision mortelle.'),
(N'Chat-Boum', 20, 70, N'images/chat_boum.png', N'Un chat explosif, prêt à tout faire sauter.'),
(N'Chatmarai', 75, 25, N'images/chatmarai.png', N'Un chat samouraï avec un katana affûté.'),
(N'Chatling Gun', 50, 40, N'images/chatling_gun.png', N'Un chat armé d''une mitrailleuse.'),
(N'Chatrayeur', 60, 35, N'images/chatrayeur.png', N'Un chat qui tire des rayons lasers.'),
(N'Chatcheuse', 40, 50, N'images/chatcheuse.png', N'Un chat avec des bombes collantes.'),
(N'Chagladiateur', 85, 20, N'images/chagladiateur.png', N'Un chat combattant dans l''arène.'),
(N'Charcher', 45, 45, N'images/charcher.png', N'Un chat archer avec des flèches enchantées.'),
(N'Chatmando', 50, 35, N'images/chatmando.png', N'Un chat soldat d''élite avec des techniques de commando.'),
(N'Chatley Davidson', 60, 40, N'images/chatley_davidson.png', N'Un chat motard avec une chaîne en métal.'),
(N'Chatnoir', 30, 55, N'images/chatnoir.png', N'Un chat qui frappe dans l''ombre.'),
(N'Chatpocalypse', 70, 20, N'images/chatpocalypse.png', N'Un chat infirmier qui soigne ses alliés sur le champ de bataille.'),
(N'Chatborg', 65, 35, N'images/chatborg.png', N'Un chat mi-machine, mi-félin.'),
(N'Chatactik', 50, 45, N'images/chatactik.png', N'Un chat stratège qui domine avec l''intelligence.'),
(N'Chalakazam', 50, 40, N'images/chalakazam.png', N'Un chat électrique qui électrocute ses ennemis.'),
(N'Chat-Croc', 90, 15, N'images/chat_croc.png', N'Un chat berzerker qui attaque avec une fureur sauvage.'),
(N'Chatfusil', 55, 35, N'images/chatfusil.png', N'Un chat armé d''un fusil d''assaut.'),
(N'Chatière', 40, 50, N'images/chatière.png', N'Un chat archer contrôlant les éléments.'),
(N'Chatraqueur', 45, 40, N'images/chatraqueur.png', N'Un chat chasseur qui traque ses proies.'),
(N'Chat-astrophe', 80, 20, N'images/chat_astrophe.png', N'Un chat furieux qui cause des destructions massives.'),
(N'Chatécul', 60, 25, N'images/chatécul.png', N'Un chat guérisseur qui aide ses alliés dans la bataille.'),
(N'Chategik', 50, 45, N'images/chategik.png', N'Un chat tacticien qui utilise la stratégie pour gagner.'),
(N'Chatpulte', 45, 50, N'images/chatpulte.png', N'Un chat grenadier avec une précision parfaite.'),
(N'Chalameche', 40, 55, N'images/chalameche.png', N'Un chat qui manie un lance-flammes.'),
(N'Chatleroy', 85, 20, N'images/chatleroy.png', N'Un chat enragé, devenant plus fort à chaque coup.'),
(N'Chatkipu', 35, 60, N'images/chatkipu.png', N'Un chat télépathe qui attaque mentalement.'),
(N'Chatbombe', 50, 40, N'images/chatbombe.png', N'Un chat qui pose des pièges explosifs.'),
(N'Chat éclair', 35, 55, N'images/chat_eclair.png', N'Un chat rapide qui frappe avant ses ennemis.'),
(N'Chatmène', 60, 35, N'images/chatmène.png', N'Un chat qui contrôle le feu.'),
(N'Chatpolaire', 65, 30, N'images/chatpolaire.png', N'Un chat qui gèle ses ennemis avec son souffle glacial.'),
(N'Chat de Nuit', 30, 60, N'images/chat_de_nuit.png', N'Un chat qui frappe depuis l''ombre.'),
(N'Chatfantome', 40, 55, N'images/chatfantome.png', N'Un chat avec une lame intangible.'),
(N'Chabagarreur', 75, 25, N'images/chabagarreur.png', N'Un chat bagarreur qui aime les combats rapprochés.'),
(N'Chathack', 35, 60, N'images/chathack.png', N'Un chat qui pirate les systèmes ennemis pour les désactiver.');
GO
