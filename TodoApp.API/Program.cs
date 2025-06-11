using Microsoft.EntityFrameworkCore;
using System;
using TodoApp.API.Extensions;
using TodoApp.Data.DataModels.Entities;
using TodoApp.Data.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:TodoAppDb"]));

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityApiEndpoints<UserEntity>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
