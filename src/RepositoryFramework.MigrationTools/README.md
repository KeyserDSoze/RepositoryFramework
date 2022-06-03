## Migration Tools

You need to create a base model as a bridge for your migration. After that you can use the two repositories with repository pattern to help yourself with the migration from a old storage to a brand new storage.

### Sample with in memory integration (From UnitTest)
For instance you can create an in memory repository and after you add a migration source repository in this way

    .AddRepositoryInMemoryStorage<MigrationUser, string>()
    .ToServiceCollection()
    .AddMigrationSource<MigrationUser, string, MigrationFrom>(x => x.NumberOfConcurrentInserts = 2)

Now you may use the interface in DI

    IMigrationManager<MigrationUser, string> migrationService

and let the sorcery happen

    var migrationResult = await _migrationService.MigrateAsync(x => x.Id!, true);