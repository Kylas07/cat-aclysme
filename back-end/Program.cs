using CatAclysmeApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la connexion à la base de données pour CatAclysmeContext
builder.Services.AddDbContext<CatAclysmeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services pour les API controllers (sans vues)
builder.Services.AddControllers();

// Ajouter CORS pour permettre les appels depuis le frontend Vue.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8081") // Adresse de ton app Vue.js
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Activer Swagger pour la documentation de l'API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajouter la configuration de la journalisation
builder.Logging.ClearProviders(); // Facultatif, pour retirer les autres providers
builder.Logging.AddConsole();     // Ajouter la sortie dans la console
builder.Logging.AddDebug(); 
// Configurer Serilog pour la journalisation dans la console et dans un fichier
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Affichage des logs dans la console
    .WriteTo.File("Logs/app-log-.txt", rollingInterval: RollingInterval.Day) // Écriture des logs dans un fichier
    .CreateLogger();

// Utiliser Serilog comme logger principal
builder.Host.UseSerilog();

// Ajouter les services de session (si nécessaire)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Durée de vie de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configurer le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatAclysmeApp API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Activer CORS pour les appels du frontend
app.UseCors("AllowVueApp");

// Activer la gestion des sessions
app.UseSession();

app.UseAuthorization();

// Map API controllers uniquement (plus besoin de MapControllerRoute pour les vues)
app.MapControllers();

app.Run();
