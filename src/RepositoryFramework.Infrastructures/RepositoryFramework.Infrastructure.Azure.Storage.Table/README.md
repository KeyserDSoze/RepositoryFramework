### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

## Integration with Azure TableStorage and Repository Framework

    builder.Services
        .AddRepositoryInTableStorage<User, string>(builder.Configuration["ConnectionString:Storage"]);

You found the IRepository<User, string> in DI to play with it.

### Automated api with Rystem.RepositoryFramework.Api.Server package
With automated api, you may have the api implemented with your tablestorage integration.
You need only to add the AddApiForRepositoryFramework

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.AddApiForRepositoryFramework();
