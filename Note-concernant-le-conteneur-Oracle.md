<img src="https://r2cdn.perplexity.ai/pplx-full-logo-primary-dark%402x.png" style="height:64px;margin-right:32px"/>

***

## Guide de connexion à la base Oracle (Docker + FREEPDB1)

Ce projet utilise **Oracle AI Database 26ai Free** dans un conteneur Docker (`gvenzl/oracle-free`). La base applicative se trouve dans la PDB `FREEPDB1`.[^1][^2]

***

### 1. Démarrer et vérifier le conteneur Oracle

1. Démarrer le conteneur (exemple, à adapter si nécessaire) :

```bash
docker run -d --name oracle-free -p 1521:1521 gvenzl/oracle-free
```

2. Vérifier qu’il tourne :

```bash
docker ps
```

Vous devez voir une ligne similaire :

```text
NAMES
oracle-free
```


Les paramètres de connexion par défaut de l’image sont notamment :

- Hostname (depuis l’hôte) : `localhost`
- Port : `1521`
- Service name : `FREEPDB1`[^1][^2]

***

### 2. Paramètres de connexion standards

Pour se connecter à la PDB utilisée par l’application :

- Host : `localhost`
- Port : `1521`
- Service name : `FREEPDB1`
- Utilisateur admin : `system`
- Mot de passe admin : `Oracle123` (ou celui défini au lancement du conteneur)

La cible de connexion est donc :

```text
localhost:1521/FREEPDB1
```


***

### 3. Utiliser SQL*Plus dans le conteneur

`sqlplus` est disponible **dans** le conteneur Oracle. Pour l’utiliser :

1. Ouvrir un shell interactif dans le conteneur :

```bash
docker exec -it oracle-free bash
```

2. Se connecter en `system` sur la PDB `FREEPDB1` :

```bash
sqlplus system/Oracle123@localhost:1521/FREEPDB1
```

3. Vérifier que l’on est bien dans la bonne PDB :

```sql
SHOW CON_NAME;
```

Le résultat attendu est :

```text
CON_NAME
------------------------------
FREEPDB1
```[^3][^4]

```


***

### 4. Utilisateur applicatif EF_USER

L’application utilise un utilisateur dédié `EF_USER` dans la PDB `FREEPDB1`.

#### 4.1 Vérifier l’existence de EF_USER

Dans SQL*Plus (connecté en `system` sur `FREEPDB1`) :

```sql
SELECT username, account_status
FROM dba_users
WHERE username = 'EF_USER';
```

- Si `EF_USER` apparaît avec `ACCOUNT_STATUS = OPEN`, le compte est disponible.
- Sinon, il faut le créer ou le réinitialiser.[^5][^6]


#### 4.2 Créer ou recréer EF_USER

Toujours dans SQL*Plus (PDB `FREEPDB1`) :

```sql
DROP USER EF_USER CASCADE;
```

```sql
CREATE USER EF_USER IDENTIFIED BY 1234;
GRANT CONNECT, RESOURCE TO EF_USER;
ALTER USER EF_USER QUOTA UNLIMITED ON USERS;
```

- `CREATE USER` crée le schéma applicatif.
- `GRANT CONNECT, RESOURCE` donne les privilèges de base (connexion, création d’objets).
- `ALTER USER ... QUOTA UNLIMITED ON USERS` autorise la création d’objets dans le tablespace USERS.[^7][^8][^9]


#### 4.3 Réinitialiser le mot de passe de EF_USER

Si `EF_USER` existe déjà mais que la connexion échoue (ORA‑01017), on peut simplement réinitialiser le mot de passe :

```sql
ALTER USER EF_USER IDENTIFIED BY 1234;
```

Le changement est immédiat.[^10][^11]

#### 4.4 Tester la connexion EF_USER

Sortir de SQL*Plus :

```sql
EXIT;
```

Puis, dans le shell du conteneur :

```bash
sqlplus EF_USER/1234@localhost:1521/FREEPDB1
```

Si la connexion réussit, l’utilisateur applicatif est correctement configuré pour la PDB `FREEPDB1`.[^12][^13]

***

### 5. Connexion depuis les IDE (Rider, etc.)

Pour ajouter une connexion dans un IDE (JetBrains Rider, SQL Developer, etc.) :

- Host : `localhost`
- Port : `1521`
- Type : **Service name** (et non pas SID)
- Service name : `FREEPDB1`
- User : `EF_USER` (pour les opérations applicatives) ou `system` (pour l’admin)
- Password : `1234` pour `EF_USER` (ou le mot de passe choisi)

Cela correspond à la même cible que celle utilisée par SQL*Plus (`localhost:1521/FREEPDB1`).[^2][^4][^14]

***

### 6. Chaîne de connexion .NET / EF Core

Pour l’application .NET (EF Core, ODP.NET, etc.), utilisez une chaîne de connexion du type :

```csharp
User Id=EF_USER;
Password=1234;
Data Source=localhost:1521/FREEPDB1;
```

Points importants :

- Le format `host:port/service_name` dans `Data Source` permet de viser directement la PDB `FREEPDB1`.
- Les informations doivent être cohérentes avec celles validées via SQL*Plus (`EF_USER/1234`).[^2][^14][^15]

***

### 7. Erreur fréquente : ORA‑01017 (invalid username/password)

L’erreur suivante peut apparaître :

```text
ORA-01017: invalid username/password; logon denied
```

Causes typiques :

- Mot de passe incorrect (y compris casse différente).
- Utilisateur non créé dans la PDB `FREEPDB1`.
- Connexion pointant vers un mauvais service (ex. pas `FREEPDB1`).[^16][^12]

Procédure de résolution recommandée :

1. Vérifier la connexion en SQL*Plus avec les mêmes identifiants que l’application (depuis le conteneur) :

```bash
sqlplus EF_USER/1234@localhost:1521/FREEPDB1
```

2. Si cela échoue, se connecter en `system` à `FREEPDB1` puis :

```sql
SHOW CON_NAME;
```

pour confirmer `FREEPDB1`, puis :

```sql
SELECT username, account_status
FROM dba_users
WHERE username = 'EF_USER';
```

3. Si besoin, réinitialiser le mot de passe :

```sql
ALTER USER EF_USER IDENTIFIED BY 1234;
```


Une fois la connexion SQL*Plus fonctionnelle avec `EF_USER/1234`, aligner la chaîne de connexion de l’application sur ces paramètres.[^12][^16]

***

### 8. Workflow standard pour l’équipe

Pour travailler tous de la même façon :

1. Démarrer / vérifier le conteneur Oracle (`oracle-free`).
2. Ouvrir un shell dans le conteneur :

```bash
docker exec -it oracle-free bash
```

3. Se connecter en `system` sur `FREEPDB1` :

```bash
sqlplus system/Oracle123@localhost:1521/FREEPDB1
```

4. Créer / vérifier / réparer l’utilisateur `EF_USER` (sections 4.1 à 4.3).
5. Tester la connexion :

```bash
sqlplus EF_USER/1234@localhost:1521/FREEPDB1
```

6. Configurer IDE et application avec :
    - Host : `localhost`
    - Port : `1521`
    - Service name : `FREEPDB1`
    - User / Password : `EF_USER` / `1234`
    - Chaîne de connexion .NET :

```csharp
User Id=EF_USER;
Password=1234;
Data Source=localhost:1521/FREEPDB1;

```

# Option 2 pour lancer le conteneur de la BD Oracle (Docker + FREEPDB1)**

### Étape 1 : Positionnement dans le dossier du projet**

Ouvrez votre terminal (par exemple Git Bash ou une invite de commande) et naviguez dans le répertoire racine de la configuration :
Bash

    cd "/c/PROJET SYNTHESE/oracle-init"

### Étape 2 : Construction et lancement**

Exécutez la commande combinée de construction de l'image et de mise en route du service :
Bash

    docker compose up --build
__L'argument `--build` force Docker à lire le `Dockerfile` pour ré-inclure le script `01_init.sql` même si le script a été modifié récemment.__

4. Validation du bon fonctionnement (Suivi des Logs)
----------------------------------------------------

L'initialisation d'une base de données Oracle peut prendre entre 1 et 3 minutes selon la puissance de la machine. Durant le défilement des lignes de texte dans le terminal, surveillez ces étapes clés :
1.  ****Phase de copie**** : Docker confirme la création de l'image locale nommée `my-custom-oracle-free`.

2.  ****Phase d'exécution SQL**** : Recherchez la ligne indiquant la prise en compte de votre script : `oracle-free | Executing /container-entrypoint-initdb.d/01_init.sql`

3.  ****Phase de succès**** : Le conteneur est officiellement prêt à recevoir des connexions externes de vos applications lorsque le message suivant s'affiche : `oracle-free | DATABASE IS READY TO USE!`

5. Paramètres de connexion à la base de données
**-----------------------------------------------**

Une fois le conteneur opérationnel, vous pouvez y connecter vos applications clientes (comme un client .NET ou un outil de gestion de base de données) en utilisant les identifiants configurés :
*   ****Hôte (Host)**** : `localhost`

*   ****Port**** : `1521`

*   ****Mot de passe de l'administrateur (SYS / SYSTEM)**** : `Oracle123`

*   ****Nom du service (Service Name)**** : `FREEPDB1` __(nom de la base de données enfichable par défaut)__

<span style="display:none">```[^15][^2][^1][^17][^18][^19][^20][^21][^22][^23][^24][^25][^26]</span>

<div align="center">⁂</div>

[^1]: https://github.com/gvenzl/oci-oracle-free

[^2]: https://hub.docker.com/r/gvenzl/oracle-free

[^3]: https://docs.oracle.com/en/database/oracle/oracle-database/12.2/admin/viewing-information-about-cdbs-and-pdbs-with-sql-plus.html

[^4]: https://oralytics.com/23c/23c-free-on-docker/

[^5]: https://www.oracletutorial.com/oracle-administration/oracle-list-users/

[^6]: https://docs.oracle.com/en/database/oracle/oracle-database/18/refrn/DBA_USERS.html

[^7]: https://docs.oracle.com/cd/B13789_01/server.101/b10759/statements_8003.htm

[^8]: https://blog.devart.com/how-to-create-oracle-user.html

[^9]: https://docs.oracle.com/en/database/oracle/oracle-database/19/dbseg/configuring-authentication.html

[^10]: https://docs.oracle.com/en/database/oracle/oracle-database/19/sqlrf/ALTER-USER.html

[^11]: https://docs.oracle.com/cd/B13789_01/server.101/b10759/statements_4003.htm

[^12]: https://learnomate.org/ora-01017-invalid-username-password-logon-denied-3/

[^13]: https://docs.oracle.com/error-help/db/ora-01017/

[^14]: https://www.cleverence.com/articles/oracle-documentation/connecting-to-oracle-database-and-exploring-it-4827/

[^15]: https://stackoverflow.com/questions/76050929/connecting-to-an-oracle-pluggable-database-using-connection-string-in-net-core

[^16]: https://pitstop.manageengine.com/portal/en/kb/articles/ora-01017-invalid-username-password-error

[^17]: https://stackoverflow.com/questions/77681461/cannot-connect-to-oracle-database-within-a-spring-boot-application-using-docker

[^18]: https://mattmulvaney.hashnode.dev/using-23ai-free-docker-container-alongside-23c-free-container

[^19]: https://blogs.oracle.com/coretec/oracle-database-23c-development-edition-on-docker

[^20]: https://ahmedfattah.com/2023/03/19/ora-01017-invalid-username-password-logon-denied/

[^21]: https://connor-mcdonald.com/2023/04/04/23c-for-developers-is-here/

[^22]: https://github.com/gvenzl/setup-oracle-free

[^23]: https://stackoverflow.com/questions/14476875/ora-01017-invalid-username-password-when-connecting-to-11g-database-from-9i-clie

[^24]: https://www.geraldonit.com/oracle-xe-docker-images/

[^25]: https://hub.docker.com/r/gvenzl/oracle-free/tags

[^26]: https://doyensys.com/blogs/ora-01017-invalid-username-password-logon-denied/

