using Messager_Project.Model;
using Messager_Project.Model.DataSeeds;
using Messager_Project.Model.Enteties;
using Messager_Project.Repository.Emote;
using Messager_Project.Repository.Users;
using Microsoft.EntityFrameworkCore;

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
