using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Executors;
using PostgreWebClient.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddSingleton<IConnectionService, ConnectionService>()
    .AddSingleton<ICommandService, CommandService>()
    .AddSingleton<IDatabaseInfoService, DatabaseInfoService>()
    .AddSingleton<IConnectionMaker, ConnectionMaker>()
    .AddSingleton<IQueryPipeline, QueryPipelineService>()
    .AddSingleton<IPaginationService, PaginationService>()
    .AddTransient<ICommandExecutor, NpgsqlCommandExecutor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();