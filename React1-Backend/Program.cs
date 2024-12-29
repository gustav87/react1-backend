using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using React1_Backend.Account;
using React1_Backend.Alibaba;
using React1_Backend.Contact;
using React1_Backend.Contracts;
using React1_Backend.Filters.ActionFilters;
using React1_Backend.Paypal;
using React1_Backend.S3;
using React1_Backend.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add the Microsoft.AspNetCore.Http.HttpContextAccessor service as a singleton.
// .AddHttpContextAccessor() is equivalent to using .AddSingleton(), shown commented out below.
builder.Services.AddHttpContextAccessor();
// builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add the System.Net.Http.IHttpClientFactory and related services to the IServiceCollection
builder.Services.AddHttpClient();

// Add compression
// builder.Services.AddResponseCompression(options =>
// {
//     options.EnableForHttps = true;
// });

// Add controllers
builder.Services.AddControllers();

// Add FluentValidation.AspNetCore
builder.Services.AddValidatorsFromAssemblyContaining<ContactValidator>();

// Add BooksService with MongoDB
builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();

// Add AccountService with MongoDB
builder.Services.Configure<AccountDatabaseSettings>(builder.Configuration.GetSection("AccountDatabase"));
builder.Services.AddSingleton<AccountService>();

// Configure and add MongoDB service
string MONGO_CONNECTION_STRING = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ?? "mongodb://localhost:27017";
MongoClientSettings mongoSettings = MongoClientSettings.FromConnectionString(MONGO_CONNECTION_STRING);
mongoSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
var mongoClient = new MongoClient(mongoSettings);
builder.Services.AddSingleton(mongoClient);

// Add singleton services
builder.Services.AddSingleton<S3Service>();
builder.Services.AddSingleton<AlibabaService>();
builder.Services.AddSingleton<PaypalService>();
builder.Services.AddSingleton<ContactService>();

// Add scoped or transient services
builder.Services.AddScoped<ActionFilterExample>();
builder.Services.AddScoped<AsyncAdminTokenFilter>();
builder.Services.AddScoped<EndpointDisabledFilter>();

// Add repositories
builder.Services.AddSingleton<ContactRepository>();
builder.Services.AddSingleton<AccountRepository>();

// Add Swagger. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.UseResponseCompression();

// Enable CORS
app.UseCors(builder =>
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials());

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
