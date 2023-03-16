using DataAccess;
using Microsoft.EntityFrameworkCore;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DbContext, AppDbContext>(optionsAction =>
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddIdentity();

builder.Services.AddServices();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAPIControllers();

builder.Services.AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();

    using var scope = ((IApplicationBuilder)app).ApplicationServices.CreateScope();

    var appDbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

    await appDbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();