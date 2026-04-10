using InboxToKindle.Functions.Options;
using InboxToKindle.Functions.Repositories;
using InboxToKindle.Functions.Services.Implementations;
using InboxToKindle.Functions.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.Configure<SendGridOptions>(context.Configuration.GetSection("SendGrid"));
        services.Configure<SupabaseOptions>(context.Configuration.GetSection("Supabase"));
        services.Configure<KindleDeliveryOptions>(context.Configuration.GetSection("KindleDelivery"));

        services.AddHttpClient();
        services.AddSingleton<ISupabaseRepository, SupabaseRepository>();
        services.AddSingleton<IHtmlProcessingService, HtmlProcessingService>();
        services.AddSingleton<IEpubGenerationService, EpubGenerationService>();
        services.AddSingleton<IKindleDeliveryService, KindleDeliveryService>();
        services.AddSingleton<INewsletterPipelineService, NewsletterPipelineService>();
    })
    .Build();

await host.RunAsync();
