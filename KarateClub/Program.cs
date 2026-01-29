using KarateClubDataAccessLayer;

var builder = WebApplication.CreateBuilder(args);


clsDataSetting.ConnectionString =
    builder.Configuration?.GetConnectionString("DefaultConnection");

// Add services
builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
