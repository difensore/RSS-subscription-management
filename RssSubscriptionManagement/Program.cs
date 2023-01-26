using Microsoft.EntityFrameworkCore;
using DAL.Models;
using RssSubscriptionManagement.Interfaces;
using RssSubscriptionManagement.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RssSubscriptionManagementContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
    opts.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<RssSubscriptionManagementContext>();
builder.Services.AddTransient<IDataProvider, DataProvider>();
builder.Services.AddTransient<IFeedsConvertor, RSSFeedsTransform>();
builder.Services.AddTransient<IIdentityProvider, IdentityProvider>();
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
