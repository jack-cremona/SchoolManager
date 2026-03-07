# SchoolManager

SchoolManager è un'applicazione backend sviluppata con **ASP.NET Core Web API** su **.NET 10**, nata per gestire in modo strutturato e completo un sistema informativo scolastico.
Consente la gestione di studenti, insegnanti, corsi, materie e iscrizioni mettendo a disposizione dei client un'interfaccia API RESTful documentata e facilmente interfacciabile.

## 🚀 Tecnologie e Strumenti Utilizzati

Il progetto sfrutta le funzionalità e i pacchetti più recenti dell'ecosistema .NET:
- **.NET 10**: Framework di base per alte prestazioni e moderna gestione del codice.
- **ASP.NET Core Web API**: Per l'esposizione di endpoint RESTful.
- **Entity Framework Core 10**: ORM utilizzato per l'accesso ai dati, con supporto completo alle Migrations.
- **SQL Server**: Database relazionale utilizzato per la persistenza dei dati.
- **Scalar per OpenAPI**: Per la visualizzazione interattiva della documentazione API e test degli endpoint direttamente via browser.

## 🏗️ Architettura del Progetto

Il progetto è focalizzato sull'applicazione Web API principale (`SchoolManager`):

- **Controllers**: Espongono gli endpoint REST raggruppati per entità. Ciascun controller gestisce una categoria di informazioni permettendo operazioni CRUD (Create, Read, Update, Delete) di base e avanzate includendo dati correlati.
- **Data (`SchoolDbContext`)**: Contiene la definizione del contesto database e la configurazione dei DbSet (Tabelle) usando Entity Framework Core. Qui risiedono anche i modelli (Entities) fondamentali.
- **DTOs (Data Transfer Objects)**: Utilizzati per scambiare dati in input/output nascondendo l'effettiva implementazione delle entità di dominio ai client esterni e permettendo di prevenire l'*over-posting*. Viene utilizzato un servizio personalizzato e stateless (`Mapper`) per gestire il mapping da Modelli a DTO.
- **Migrations**: Storico dei cambiamenti dello schema del database autogenerato da EF Core.

## 🗂️ Entità Gestite

Il sistema ruota attorno a diverse entità di dominio correlate:
- **Teacher (Insegnante)**
- **Student (Studente)**
- **Course (Corso)**
- **Subject (Materia)**
- **Module (Modulo / Parte del corso)**
- **Assignment (Assegnazione/Compiti)**
- **Enrollment (Iscrizione)**
- **Competence (Competenza tecnica o trasversale)**

Molto spesso i Controller offrono endpoint specifici come `Get...WithDetails` che sfruttano l'Eager Loading di EF Core (`.Include()`) per restituire grafi di oggetti completi attraverso i DTO.

## ⚙️ Prerequisiti

Per eseguire il progetto sono richiesti:
- [SDK .NET 10](https://dotnet.microsoft.com/download)
- Un'istanza o un server di **SQL Server** in esecuzione.
- IDE suggeriti: Visual Studio 2022 o Visual Studio Code (con estensione C# Dev Kit).

## 🚀 Installazione e Avvio

1. **Configurazione stringa di connessione**
   Apri il file `appsettings.json` o `appsettings.Development.json` all'interno della cartella `SchoolManager` e assicurati che la stringa `Default` sotto `ConnectionStrings` punti correttamente alla tua istanza locale di SQL Server.

2. **Applicazione delle Migrazioni**
   Apri il terminale, posizionati sulla cartella contenente `SchoolManager.csproj` ed esegui il comando:
   ```bash
   dotnet ef database update
   ```
   *Nota: qualora non avessi i tool di EF Core installati a livello globale, esegui prima: `dotnet tool install --global dotnet-ef`*

3. **Avvio dell'applicazione**
   Dal terminale:
   ```bash
   dotnet run --project SchoolManager
   ```
   L'API si avvierà su porta HTTPS/HTTP locale (es. `https://localhost:xxxx`). 
   Puoi navigare alla root `/scalar/v1` per visualizzare l'interfaccia interattiva dell'API, grazie a Scalar e Microsoft OpenAPI.
