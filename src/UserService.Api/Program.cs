using Orchestration.ServiceDefaults;
using UserService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
GeneralServiceExtensions.ConfigureSerilog(builder);

var app = builder.ConfigureServices()
                 .ConfigurePipeline(builder);

app.Run();