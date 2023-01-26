using Microsoft.EntityFrameworkCore;
using DAL.Models;
using RssSubscriptionManagement.Interfaces;
using RssSubscriptionManagement.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RssSubscriptionManagementContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddTransient<IDataProvider, DataProvider>();
builder.Services.AddTransient<IFeedsConvertor, RSSFeedsTransform>();
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
