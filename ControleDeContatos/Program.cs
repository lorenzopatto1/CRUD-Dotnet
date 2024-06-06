using ControleDeContatos.Data;
using ControleDeContatos.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

// DI

builder.Services.AddDbContext<BancoContext>
    (options => options.UseSqlServer
    (builder.Configuration.GetConnectionString
    ("Database"))
    );

builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
