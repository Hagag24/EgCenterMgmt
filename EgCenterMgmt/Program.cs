var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwagger(builder.Configuration);

builder.Services.AddDependencyInjection(builder.Configuration);

await builder.Services.AddTenancyAsync(builder.Configuration);

builder.Services.AddAuthenticationClean(builder.Configuration);

var app = builder.Build();

await builder.Services.AddSeeding(builder.Configuration, app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();
