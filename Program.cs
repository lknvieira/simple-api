using Microsoft.EntityFrameworkCore;
using simple_api.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options => options.UseInMemoryDatabase("School"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
