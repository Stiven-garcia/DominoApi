using Domino.Application;
using Domino.Controllers;
using Domino.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Configura esto a "true" en producci�n
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Valida el "issuer" del token
            ValidateAudience = true, // Valida el "audience" del token
            ValidateLifetime = true, // Valida la fecha de expiraci�n del token
            ValidateIssuerSigningKey = true, // Valida la clave de firma del token
            ValidIssuer = "*", // Establece el "issuer" v�lido
            ValidAudience = "*", // Establece el "audience" v�lido
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("clave-secreta-api")) // Establece la clave de firma
        };
    });


builder.Services.AddSwaggerGen(options =>
{
    // Configuraci�n general de Swagger

    // Agrega la documentaci�n de seguridad para JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT con la palabra 'Bearer' seguida de un espacio.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Agrega un requisito de seguridad para JWT
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Configuraci�n adicional de Swagger
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IDominoServices, DominoServices>();
builder.Services.AddHttpClient<DominoController>()
    .AddHttpMessageHandler<TokenValidationHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
});

app.UseHttpsRedirection();



app.UseAuthentication();

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = "No est� autorizado para acceder a este recurso."
        }));
    });
});
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
