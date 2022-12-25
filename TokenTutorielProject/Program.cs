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

        //Tokený kim daðýtýyor.
        ValidIssuer = builder.Configuration["Token:Issuer"],

        //Oluþturulan tokený hangi clientlarýn kullanacaðýný belirlediðimiz alandýr.
        ValidAudience = builder.Configuration["Token:Audience"],

        //Oluþturulan token deðeri uygulamamýza ait bir deðer olduðunu anlamak için kullanýrýz.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

        //Buraya verilecek süre ne olursa Token süresine eklenir.
        //Aralarýnda zaman farký olan farklý lokasyonlardaki sunuculardaki uygulamaya gelen kullanýcý için Expire süresi içerisinde her iki sunucuda da tokený kullanabilsin.
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
