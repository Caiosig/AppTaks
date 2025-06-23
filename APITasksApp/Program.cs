using APITasksApp;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Add services to the container.

builder.AddServices();
builder.AddDbContext();
builder.AddFluentValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
