using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PDFShelf.Api.Data;
using PDFShelf.Api.Models;
using PDFShelf.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

//Adiciona controladores e endpoints
builder.Services.AddControllers();

//Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Mapeia endpoints
app.MapTestDbEdnpoints();


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


