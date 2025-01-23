using UserService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
HostingExtensions.ConfigureSerilog(builder);

var app = builder.ConfigureServices()
                 .ConfigurePipeline(builder);

app.Run();
