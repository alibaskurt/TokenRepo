using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        //Token� kim da��t�yor.
        ValidIssuer = builder.Configuration["Token:Issuer"],

        //Olu�turulan token� hangi clientlar�n kullanaca��n� belirledi�imiz aland�r.
        ValidAudience = builder.Configuration["Token:Audience"],

        //Olu�turulan token de�eri uygulamam�za ait bir de�er oldu�unu anlamak i�in kullan�r�z.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

        //Buraya verilecek s�re ne olursa Token s�resine eklenir.
        //Aralar�nda zaman fark� olan farkl� lokasyonlardaki sunuculardaki uygulamaya gelen kullan�c� i�in Expire s�resi i�erisinde her iki sunucuda da token� kullanabilsin.
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
