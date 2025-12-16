using SampleManager.API.Configuration;
using SampleManager.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LibrarySettings>(
    builder.Configuration.GetSection("LibrarySettings")
);

// Add services to the container
builder.Services.AddSingleton<IFileSystemService, FileSystemService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
