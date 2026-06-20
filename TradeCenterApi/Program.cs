using DataLibrary.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=TradeCenterDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0"));

var app = builder.Build();

// Настройка HTTP конвейера
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();