# Extension methods for setting up Microsoft SQL Server caching for ASP.NET Core

## Usage

### Step 1

Install NuGet package
```bash
dotnet add package Caching.SqlServer.Infastructure --version 1.0.1
```

### Step 2

Register services after registering Sql Server Distributed caching
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("CacheDbConnection");
                options.SchemaName = "app";
                options.TableName = "Cache";
            }).AddSqlServerCachingInfrastructure();
            ...
        }
```

### Step 3

Initiate setup from the pipeline
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
            app.SetupSqlServerCachingInfrastructure();
            ...
        }
```
