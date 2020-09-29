﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer {
  public class Startup {
    public IWebHostEnvironment Environment { get; }

    public Startup (IWebHostEnvironment environment) {
      Environment = environment;
    }

    public void ConfigureServices (IServiceCollection services) {
      // uncomment, if you want to add an MVC-based UI
      services.AddControllersWithViews ();

      // var builder = services.AddIdentityServer (options => {
      //     options.EmitStaticAudienceClaim = true;
      //   })
      //   .AddInMemoryIdentityResources (Config.IdentityResources)
      //   .AddInMemoryApiScopes (Config.ApiScopes)
      //   .AddInMemoryClients (Config.Clients)
      //   .AddTestUsers (TestUsers.Users);
      // not recommended for production - you need to store your key material somewhere secure
      // builder.AddDeveloperSigningCredential ();

      var migrationsAssembly = typeof (Startup).GetTypeInfo ().Assembly.GetName ().Name;
      const string connectionString = @"Server=localhost;Database=IdentityServer4Demo;User Id=sa;Password=P@ssword1234;";
      services.AddIdentityServer ()
        .AddTestUsers (TestUsers.Users)
        .AddConfigurationStore (options => {
          options.ConfigureDbContext = b => b.UseSqlServer (connectionString, sql => sql.MigrationsAssembly (migrationsAssembly));
        })
        .AddOperationalStore (options => {
          options.ConfigureDbContext = b => b.UseSqlServer (connectionString, sql => sql.MigrationsAssembly (migrationsAssembly));
        });

    }

    public void Configure (IApplicationBuilder app) {
      if (Environment.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      }

      // uncomment if you want to add MVC
      app.UseStaticFiles ();
      app.UseRouting ();

      app.UseIdentityServer ();

      // uncomment, if you want to add MVC
      app.UseAuthorization ();
      app.UseEndpoints (endpoints => {
        endpoints.MapDefaultControllerRoute ();
      });
    }
  }
}