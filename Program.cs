using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TailwindApi.Middleware;
using TailwindApi.Service;
using TailwindApi.Service.IService;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication((builder) =>
    {
        builder.UseMiddleware<ExceptionHandlerMiddlerware>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProductService, ProdcutService>();
        services.AddSingleton<CosmosClient>((provider) =>
        {
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            return cosmosClient;
           
        });


    })
    .Build();

host.Run();

 