using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Core.Repositories;
using RepositoryPattern.EF.Data;
using RepositoryPattern.EF.Mapper;
using RepositoryPattern.EF.Repositories;
using Scalar.AspNetCore;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("cs"),
     b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.AddTransient<IUnitOfWork , UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MppingProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
