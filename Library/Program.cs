using FluentValidation;
using Library.ActionFilters;
using Library.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
"/nlog.config"));

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureIdentity();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureValidation();
builder.Services.ConfigureMailjet(builder.Configuration);
builder.Services.ConfigureLoggerService();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddSession();

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LocalizationRouteConstraint));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/PageNotFound");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// custom middleware
app.UseCultureMiddleware();

app.UseCors("CorsPolicy");
app.UseResponseCaching();

app.UseAuthorization();

app.MapControllerRoute(
    name: "localized",
    pattern: "{culture}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { culture = new LocalizationRouteConstraint() }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { culture = "en" }
);

app.Run();
