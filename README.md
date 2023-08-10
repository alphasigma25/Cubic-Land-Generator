# Cahier des charges

## Moteur de rendu

### Obligatoire

- Le système de jeu doit pouvoir afficher des cubes dans le monde, dont les faces possèdent toutes la même texture, et qui sont allignées sur une grille. La grille doit pourvoir avoir une taille supérieure a 50 000 * 50 000 * 50 000.

- Les utilisateurs doivent pouvoir controller une caméra flottant en vue libre et la déplacer librement dans l'espace.

- L'affichage doit pouvoir être fait a au moins 60 FPS sur toutes les machines des participants au projet.

### Optionnel

- Les blocs ne doivent pas forcément être des cubes complets, mais peuvent prendre différentes formes et / ou avoir des textures différentes sur les différentes faces.

- Certains blocs peuvent être lumineux, et les autres blocs doivent être éclairés en fonction des sources de lumière.

- Les parties du monde qui sont éloignées doivent être affiché avec des cubes moins précis (comme avec [distant horizons](https://www.curseforge.com/minecraft/mc-mods/distant-horizons) dans minecraft)

## Moteur de jeu

### Optionnel

- Certains des blocs doivent avoir des comportements dynamiques (arbre qui pousse au bout d'une moment, eau qui coule, ...)

## Système de génération / configuration

### Obligatoire

- Le monde doit être généré dynamiquement et de manière logique / cohérente.

- Les propriétés (texture, forme si applicable, luminosité si applicable, transparence si applicable, ...) des différents blocs doivent pouvoir être lues depuis des fichier de configuration, aucun type de bloc ne doit être hard-codé.

- La génération doit aussi pouvoir être lue depuis des fichiers de configuration, il doit pouvoir être possible de réutiliser un même bloc dans différent type de générations.

### Optionnel

- Une application indépendente doit être fournie pour éditer les fichiers de configuration

## Architecture

### Obligatoire

- L'application doit pouvoir fonctionner dans un mode client seul et un mode client / serveur

- En mode client / serveur, les client doivent pouvoir se déplacer dans le monde de manière indépendante

- Les clients doivent pouvoir modifier l'état du monde (probablement avec un système de commande étant donné qu'il n'y a pas encore de système de colision ou de raycast), et les modifications de l'état du monde doivent être partagées entre tout les clients d'un même serveur.

### Très optionnel (Je sais pas si c'est une bonne idée)

- Les clients doivent pouvoir modifier la configuration du serveur, et les modications doivent être partagés entre tout les clients
