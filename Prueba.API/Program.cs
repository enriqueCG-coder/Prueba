using Microsoft.EntityFrameworkCore;
using Prueba.API.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conString = builder.Configuration.GetConnectionString("Conexion");
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(conString));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials());

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
