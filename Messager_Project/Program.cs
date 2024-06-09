using Messager_Project.Model;
using Messager_Project.Model.DataSeeds;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Emote;
using Messager_Project.Repository.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Configuration conection to Database
var conectionString = builder.Configuration.GetConnectionString("AppDbContext");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conectionString));

// Add services to the container.

builder.Services.AddScoped<IUserRepository, MSUserRepository>(); //specifaing repository
builder.Services.AddScoped<IEmotesRepository, MSEmotesRepository>(); 
builder.Services.AddScoped<Messager_Project.Repository.MessageEmote.IMessageEmotesRepository, Messager_Project.Repository.MessageEmote.MSMessageEmotesRepository>(); 
builder.Services.AddScoped<Messager_Project.Repository.Messages.IEmotesRepository, Messager_Project.Repository.Messages.MSMessagesRepository>();
builder.Services.AddScoped<Messager_Project.Repository.UsersFriends.IMessageEmotesRepository, Messager_Project.Repository.UsersFriends.MSUserFriendsRepository>();


builder.Services.AddControllers();

//kod Micha³
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Messenger_Project", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//koniec Kodu Micha³

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Adding Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    EmotesDataSeed.Initialize(services);
    UserDataSeed.Initialize(services);
    UserFriedsDataSeed.Initialize(services);
    MessageDataSeed.Initialize(services);
    MessageEmotesDataSeed.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
