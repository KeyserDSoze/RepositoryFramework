## Integration with Azure TableStorage and Repository Framework

    builder.Services
        .AddRepositoryInTableStorage<User, string>(builder.Configuration["Storage:ConnectionString"]);

You found the IRepository<User, string> in DI to play with it.
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
