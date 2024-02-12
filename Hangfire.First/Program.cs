using Carter;
using Hangfire;
using Hangfire.Services;
using Hangfire.Storage.SQLite;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireConnection"))
);

builder.Services.AddHangfireServer();
builder.Services.AddTransient<IServicesManagement,ServiceManagement>();
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Permet de retourner un message d'erreur unique
/*app.Use(async(ctx,next) =>
{
    try
    {
        await next();
    }
    catch (Exception)
    {
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync("An error occured");
    }
});*/

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Test Hangfire",
    Authorization = new []
    {
        new HangfireCustomBasicAuthenticationFilter()
        {
            User = "admin",
            Pass = "adminTest"
        }
    }
});

app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IServicesManagement>(x=> x.SyncData(), Cron.Minutely);

app.MapCarter();

app.Run();