# Projet KeyShield par Julien Morin et Victor Girault
## Architecture du projet et fonctionnement
![Architecture du projet](img/archi.png)
## Workflow d'un appel API
![API Workflow](img/apiworkflow.png)
## Fonctionnement actuel du chiffrement des données
![Chiffrement actuel](img/chiffrement.png)
## Chiffrement qu'il aurait fallu mettre en place
Workflow :
![Chiffrement](img/chiffrement2.png)
Algorithme :
![Algo](img/algochiffrement.png)

## Etapes pour lancer le projet
### 1. Changer la connection string
Changer la connection string pour permettre la connection du projet KeyShieldDB à la base de donnée SQLServer dans `KeyShieldDB/Context/KeyShieldDbContext.cs`

### 2 .Lancer le projet API
Lancer ```dotnet run``` dans le repertoir KeyShieldAPI

### 3. Lancer le projet WebApp
Lancer ```dotnet run``` dans le repertoir KeyShieldAppWeb

### 4. Se connecter à l'application web
En allant sur ```https://localhost:7082```

## Eléments rajoutés
- Générateur de mot de passe
- Calcul force de mot de passe