using TicketsHub.Hubs;
using TicketsHub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<TicketsProcesser>();
builder.Services.AddSignalR();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SpecialistHub>("/specialisthub");
    endpoints.MapHub<AdminHub>("/adminhub");
    endpoints.MapHub<CustomerHub>("/customerhub");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
