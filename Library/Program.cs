using Library.Data.Configurations;
using Library.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureIdentity();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureValidation();
builder.Services.ConfigureMailjet(builder.Configuration);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
