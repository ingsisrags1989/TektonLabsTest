using AutoMapper;
using Domain.Repositories;
using Infrastructure.Repositories.Context;
using Infrastructure.Repositories.Generic;
using Infrastructure.Repositories.Injection;
using Infrastructure.Repositories.Product;
using Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);


builder.Services.AddScoped(typeof(DbContext), typeof(ProductContext));
builder.Services.AddScoped(typeof(IUnitOfWorkAsync), typeof(UnitOfWorkAync));
builder.Services.AddTransient(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddMvc();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var profiles = "Application";

MapperConfiguration mappingConfig = new MapperConfiguration(config =>
{
    config.AddMaps(profiles);
});


IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSwagger();
});

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
