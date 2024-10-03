-- Création de la base de données
CREATE DATABASE CatAclysmeDB;

-- Utilisation de la base de données
USE CatAclysmeDB;

-- Insertion de 30 cartes dans la table Card avec des noms basés sur des jeux de mots
INSERT INTO Card (name, health, attack, image, description) VALUES
('PasChat', 10, 5, 'paschat.webp', 'Est-ce vraiment un chat ?'),
('En Cat-imini', 7, 8, 'en_catimini.webp', 'Un chat furtif, qui se déplace en toute discrétion'),
('Chat-rlatan', 8, 7, 'chat_rlatan.webp', 'Un chat trompeur qui manipule ses adversaires'),
('Chat-r-d’assaut', 15, 6, 'chat_r_d_assaut.webp', 'Un puissant chat blindé, prêt à charger'),
('Chat-pristi', 9, 6, 'chat_pristi.webp', 'Un chat qui surprend ses ennemis par sa vivacité'),
('Chat-teau', 12, 4, 'chat_teau.webp', 'Un imposant chat qui règne sur son territoire comme un roi'),
('Chat-meau', 11, 5, 'chat_meau.webp', 'Un chat robuste qui traverse les déserts sans effort'),
('Chat-an', 13, 6, 'chat_an.webp', 'Un chat démoniaque dont le regard seul effraie'),
('Chat-hon', 8, 9, 'chat_hon.webp', 'Un chat agile et rapide comme un poisson dans l’eau'),
('Chat-riot', 14, 5, 'chat_riot.webp', 'Un chat transportant un lourd char, imparable sur le champ de bataille'),
('Chat-pardeur', 9, 8, 'chat_pardeur.webp', 'Un chat voleur, expert dans l’art de la subtilisation'),
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
('Chat-otique', 10, 8, 'chat_otique.webp', 'Un chat imprévisible, semant la confusion et le chaos sur le champ de bataille'),
('Chat-grin', 8, 7, 'chat_grin.webp', 'Un chat mélancolique, dont la tristesse se transforme en force brute'),
('Chat-piteau', 8, 7, 'chat_piteau.webp', 'Un chat clownesque sous le chapiteau, toujours prêt à surprendre ses ennemis avec ses farces');