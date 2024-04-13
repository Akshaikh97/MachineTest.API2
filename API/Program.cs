using API.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

string? connectionString = builder.Configuration.GetConnectionString("MachineTestDB");

if (connectionString != null)
{
    // Add connection string as a singleton service
    builder.Services.AddSingleton<string>(connectionString);
}
else
{
    // Handle missing connection string (e.g., throw exception, use default value, log error)
    throw new InvalidOperationException("Connection string 'MachineTestDB' not found in configuration.");
}

var app = builder.Build();

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
