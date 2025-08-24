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

            builder.Services
                 .AddCarterWithAssemblies(catalogAssembly,BasketAssembly);

            builder.Services
                .AddMediatRWithAssemblies(catalogAssembly, BasketAssembly);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services.AddMassTransitWithAssemblies(builder.Configuration,catalogAssembly, BasketAssembly);

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

            app.
                 UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            app.Run();
        }
    }
}
