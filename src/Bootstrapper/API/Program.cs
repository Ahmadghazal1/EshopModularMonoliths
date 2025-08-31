using Keycloak.AuthServices.Authentication;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
                    config.ReadFrom.Configuration(context.Configuration)
            );

            //Add services to the container.

            //common services : carter , mediatr , fluentvalidation
            var catalogAssembly = typeof(CatalogModule).Assembly;
            var BasketAssembly = typeof(BasketModule).Assembly;
            var orderingAssembly = typeof(OrderingModule).Assembly;

            builder.Services
                 .AddCarterWithAssemblies(catalogAssembly,BasketAssembly, orderingAssembly);

            builder.Services
                .AddMediatRWithAssemblies(catalogAssembly, BasketAssembly , orderingAssembly);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services.AddMassTransitWithAssemblies(builder.Configuration,catalogAssembly, BasketAssembly, orderingAssembly);

            builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
            builder.Services.AddAuthorization();

            builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

            builder.Services
                .AddExceptionHandler<CustomExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
       
            app.MapCarter();
            app.UseSerilogRequestLogging();
            app.UseExceptionHandler(options => { });
            app.UseAuthentication();
            app.UseAuthorization();

            app.
                 UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            app.Run();
        }
    }
}
