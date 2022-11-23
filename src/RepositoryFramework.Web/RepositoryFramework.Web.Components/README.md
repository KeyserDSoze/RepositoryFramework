﻿### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

## Add to service collection the UI service in your blazor DI

You have to add a service for UI

    builder.Services
        .AddRepositoryUI();

and in the Host.cshtml you have to add style and javascript files.

    <html>
        <head>
          <!-- inside of head section -->
          <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-F3w7mX95PdgyTmZZMECAngseQB83DfGTowi0iMjiWaeVhAn4FJkqJByhZMI3AhiU" crossorigin="anonymous">
          <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.15.4/css/all.css">
          <link href="_content/Blazorise/blazorise.css" rel="stylesheet" />
          <link href="_content/Blazorise.Bootstrap5/blazorise.bootstrap5.css" rel="stylesheet" />
        </head>
        <body>
          <div id="app"></div>
          <!-- inside of body section and after the div/app tag  -->
          <!-- These are the standard js dependencies this provider tipically dependes upon, but Blazorise deems these as optional as Blazorise Components should work correctly without these  -->
          <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-/bQdsTh/da6pkI1MST/rWKFNjaCP5gBSY4sEBT38Q/9RBh9AH40zEOg7Hlq2THRZ" crossorigin="anonymous"></script>
        </body>
    </html>