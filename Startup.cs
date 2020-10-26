using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ScraperTask.Services;

[assembly: FunctionsStartup(typeof(ScraperTask.Startup))]
namespace ScraperTask
{
    class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureSettings(builder);
            builder.Services.AddSingleton<IAddToBlob, AddToBlob>();
            builder.Services.AddSingleton<IAddToTable, AddToTable>();
            builder.Services.AddSingleton<IGetBlobsAsync, GetBlobsAsync>();
            builder.Services.AddSingleton<IGetBlobsByDateStr, GetBlobsByDateStr>(); 
            builder.Services.AddSingleton<IGetTablesAsync, GetTablesAsync>(); 
            builder.Services.AddSingleton<IQueryTablesByDateRange, QueryTablesByDateRange>();
            builder.Services.AddHttpClient();
        }

        private void ConfigureSettings(IFunctionsHostBuilder builder) 
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _configuration = config;

            var blobSettings = new BlobSettings()
            {
                ContainerName = _configuration["Values:ContainerName"]
            };
            var tableSettings = new TableSettings()
            {
                TableName = _configuration["Values:TableName"]
            };

            builder.Services.AddSingleton(blobSettings);
            builder.Services.AddSingleton(tableSettings);

        }
    }
}
