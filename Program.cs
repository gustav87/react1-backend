using React1_backend.Filters.ActionFilters;
using React1_backend.Contracts;
using React1_backend.Services;
using React1_backend.Account;
using React1_backend.S3;
using React1_backend.Alibaba;
using React1_backend.Paypal;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ActionFilterExample>();
builder.Services.AddScoped<AsyncAdminTokenFilter>();

// builder.Services.AddResponseCompression(options =>
// {
//     options.EnableForHttps = true;
// });

// Add controllers
builder.Services.AddControllers();

// Add BookStore MongoDB
builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();

// Add Account MongoDB
builder.Services.Configure<AccountDatabaseSettings>(builder.Configuration.GetSection("AccountDatabase"));
builder.Services.AddSingleton<AccountService>();

// Add services
builder.Services.AddSingleton<S3Service>();
builder.Services.AddSingleton<AlibabaService>();
builder.Services.AddSingleton<PaypalService>();

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
