# Extension methods for setting up Microsoft SQL Server caching for ASP.NET Core

##Usage

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

Initiate setup from the pipeline
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
            app.SetupSqlServerCachingInfrastructure();
            ...
        }
```
