# **Définition** de la cible du projet pour la session

  

## **Participants**

1. Blaise Kamuntu

2. Nicolas Plouffe

3. Eric Carrier

4. Mianta Rajeriarinalina

## Présentation du projet
Le présent projet consiste en une application bureau Windows développée pour un client qui souhaite centraliser et visualiser des données financières provenant de fichiers JSON et CSV.

### Description générale
Le client dispose déjà d'une base de données Oracle qui sert de structure d'indexation pour les données stockées dans des fichiers plats. L'objectif principale est de lui offrir une interface graphique permettant de consulter soit des prévisions socio-économiques, soit des comparaisons de données également socio-économiques pour un bassin donné. Cette logique de sélection est implémentée à l'aide du patron de conception Stratégie.

Une fois les résultats affichés dans l'interface, l'utilisateur pourra exporter les données dans plusieurs formats JSON, CSV, PDF, Excel, ou encore l'imprimer directement.

### Technologies utilisées
Le développement est réalisé en C# sur .NET 9 avec Avalonia pour la couche de présentation et Entity Framework (EF) pour l'ORM. La base de données est Oracle, qui joue un rôle d'indexation, tandis que les données brutes sont stockées dans des fichiers JSON et CSV. Le serveur de bases de données est opéré dans un conteneur Docker.

Le backend est structuré selon une architecture n-tier avec le patron Repository, ce qui permet de bien séparer la logique métier, l'accès aux données et l'exposition de l'API consommée par le client WPF. ( Wow PiFou )

### Déploiement
  Le client prend en charge  le déploiement lui-même. Les postes locaux exécutent l'interface WPF et communique avec le backend hébergé sur un serveur local. Ce serveur contient les conteneurs Docker pour la base de données et le backend. Aucun pipeline CI/CD n'est requis pour le moment.
  

## **Énumération des besoins exprimés par le client, soit en rencontre formelle ou en fonction de la documentation disponible.**

- Réalisation d’une liste de vérification de l’état du projet en fonction des besoins connus (À Faire, En Cours et Fait)
- Priorisation des besoins à combler pour la session

1. *  [ à faire ] Disposer d’une application(API) capable de se connecter à une base de données.
2. *  [ à faire ] L'API doit être en mesure de lire des fichiers de données (JSON, CSV, etc.) et, au besoin, créer un dossier ou un répertoire.
3. *    [ à faire ] Avoir la possibilité d’ouvrir plusieurs fichiers simultanément en une seule manipulation.
4. *   [ à faire ]  Faire correspondre la structure de la base de données avec les valeurs contenues dans les fichiers.
5. *    [ à faire ] Afficher le résultat de cette correspondance dans une interface graphique.
6. *    [ à faire ] Permettre l’exportation dans différents formats (JSON, CSV, Excel).
7. *    [ à faire ] Offrir la possibilité d’imprimer.
8. *    [à faire ] Documenter le projet.




## **Technologies utilisées.** 

- En développement

- [ Fait ] Diagramme de l’architecture de production (serveurs, services et liens entre eux)

-
![Schéma Projet Synthèse.png](/.attachments/Schéma%20Projet%20Synthèse-f4e39b5d-0c75-45f7-af53-2b6ad42cf24f.png)






- [X] Services utilisés (peut y en avoir plusieurs et doivent être documentés

Aucun service externe est utilisé, développement d'un logiciel desktop.

- [X] Méthodologie d’utilisation des services
Ne s'applique pas

- En production

- [x] Diagramme de l’architecture de production (serveurs, services et liens entre eux)
  -  _Même diagramme que pour le volet "Développement"_

Utilisation de conteneurs Docker et une connection à la BD

_- [X] Services utilisés (peut y en avoir plusieurs et doivent être documentés)_ 

- [X] Méthodologie d’utilisation des services
Ne s'Applique pas au cas présent.

- En déploiement

- [X] Services utilisés (peut y en avoir plusieurs et doivent être documentés)

Dans le cas présent aucune infrastructure pour le déploiement sur de l'application puisqu'il s'agit d'une application desktop. Le client prendra en charge le déploiment.

- [X] Localisation de l’hébergement des services
Il s'agira d'un serveur interne chez le client. 

- [X] Méthodologie d’utilisation des services

- [X] Estimation des coûts
Puisque le client se chargera de s'auto héberger, aucun frais. 
## **État de situation**

- Statut du projet

- [ En cours ] État actuel

    1. Ce qui est fonctionnel en production

Rien pour l'instant

    2. Ce qui est en développement

Le back-end

    3. Ce qui est en déploiement ou près du déploiement

Rien pour l'instant.
