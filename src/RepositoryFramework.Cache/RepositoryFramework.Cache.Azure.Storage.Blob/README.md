## Cache example

    builder.Services
        .AddRepositoryInBlobStorage<User, string>(builder.Configuration["Storage:ConnectionString"])
        .WithInMemoryCache(x =>
        {
            x.RefreshTime = TimeSpan.FromSeconds(20);
            x.Methods = RepositoryMethod.All;
        })
        .WithBlobStorageCache(builder.Configuration["Storage:ConnectionString"], settings: x =>
        {
            x.RefreshTime = TimeSpan.FromSeconds(120);
            x.Methods = RepositoryMethod.All;
        });

### Usage
You always will find the same interface. For instance

    IRepository<User, string> repository

or if you added a query pattern or command pattern

    IQuery<User, string> query 
    ICommand<User, string> command