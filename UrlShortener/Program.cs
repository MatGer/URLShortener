using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using UrlShortener.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LinksDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Short}/{action=Index}/{id?}");


//fallback to redirect from shortUrl to Original
app.MapFallback(async (LinksDbContext url, HttpContext context) =>
{
    var path = context.Request.Path.ToUriComponent().Trim('/');
    var match = await url.Links.FirstOrDefaultAsync(x=>x.ShortUrl==path);

    if (match == null)
    {
        return Results.BadRequest("Invalid Url");
    }

    return Results.Redirect(match.LongUrl);
});

app.Run();
