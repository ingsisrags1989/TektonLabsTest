using Application.Handlers;
using AutoMapper;
using Domain.Common.MiddlewareException;
using Domain.Repositories;
using Infrastructure.Repositories.Context;
using Infrastructure.Repositories.Generic;
using Infrastructure.Repositories.Injection;
using Infrastructure.Repositories.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//SwaggerGen
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);


builder.Services.AddScoped(typeof(DbContext), typeof(ProductContext));
builder.Services.AddTransient(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


var profiles = "Application";

MapperConfiguration mappingConfig = new MapperConfiguration(config =>
{
    config.AddMaps(profiles);
});


IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    new Assembly[] { typeof(CreateProductCommandHandler).Assembly,
    typeof(UpdateProductCommandHandler).Assembly,
    typeof(GetProductByIdQueryHandler).Assembly
    }
));


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options => options.AllowAnyHeader().AllowAnyOrigin().AllowCredentials().AllowAnyMethod());
});



var app = builder.Build();

RunMigrations(app);

app.UseRouting();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Serve the Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

static void RunMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            logger.LogInformation($"Run database migrations {typeof(ProductContext).Name}");
            var dbContext = services.GetRequiredService<ProductContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}