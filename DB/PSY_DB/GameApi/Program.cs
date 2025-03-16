using GameApi.Hubs;
using GameApi.Services;
using Microsoft.AspNetCore.Rewrite;
using PSY_DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("*") // 모든 출처 허용
               .AllowAnyHeader()   // 모든 헤더 허용
               .AllowAnyMethod();  // 모든 HTTP 메서드 허용
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PsyDbContext>();
builder.Services.AddScoped<CashService>();
builder.Services.AddSignalR();


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


app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);


app.MapHub<ChatHub>("/Chat");

app.Run();
