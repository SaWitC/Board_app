## EF Core migrations (Board)

- **Step 3.1 — Update dotnet-ef**
  - Global tool: `dotnet tool update --global dotnet-ef`
  - Local tool (if used): `dotnet tool update dotnet-ef`
  - Specific version: append `--version <VERSION>`
  
Step 3.2. To create migration for the specific database you need to go to the root directory of the solution (the folder containing Board.sln).
Examples: `cd C:\Users\user\source\repos\Board`;

Step 3.3: To create a new migration, use the following command structure:
`dotnet ef migrations add <MigrationName> --project <PathToInfrastructureProject> --startup-project <PathToMigratorProject>`.
Example: `dotnet ef migrations add InitialCreate -c BoardDbContext -o ./Migrations --startup-project ../../../Migrators/BoardDb.MigrationService`
`dotnet ef migrations add InitialCreate --project src/backend/Services/Board/Board.Infrastructure --startup-project src/backend/Services/Migrators/BoardDb.MigrationService -o Data/Migrations`
  - Notes:
    - InitialCreate: unique name of the first migration.
    - `-c <DbContext>`: fully-qualified DbContext type name.
    - `-o <OutputPath>`: folder for migration files (relative to `--project`).
    - `--startup-project`: the app that configures the DbContext and provider (here: `BoardDb.MigrationService`).

- **Step 3.4 — Apply migrations**
  - From code: run `Board.AppHost` or run `BoardDb.MigrationService` directly — it will apply any pending migrations.

- **Where migration files are located**
  - `src\backend\Services\Board\Board.Infrastructure\Data\Migrations`
