### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

## Add to service collection the UI service in your blazor DI

You have to add a service for UI

    builder.Services
        .AddRepositoryUI();

and in the Host.cshtml you have to add style and javascript files.

    <html>
        <head>
          <!-- inside of head section -->
          <partial name="RepositoryStyle" />
        </head>
        <body>
          <div id="app"></div>
          <!-- inside of body section and after the div/app tag  -->
          <partial name="RepositoryScript" />
        </body>
    </html>