using Microsoft.EntityFrameworkCore;
using DAL.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RssSubscriptionManagementContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
