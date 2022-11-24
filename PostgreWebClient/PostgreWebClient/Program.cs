using PostgreWebClient;
using PostgreWebClient.Abstractions;
using PostgreWebClient.ActivityCheck;
using PostgreWebClient.Database;
using PostgreWebClient.Extractors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddSingleton<IConnectionService, ConnectionService>()
    .AddTransient<ITableExtractor, TableExtractor>()
    .AddSingleton<ICommandService, CommandService>()
    .AddSingleton<IDatabaseInfoService, DatabaseInfoService>()
    .AddSingleton<IConnectionMaker, ConnectionMaker>()
    .AddSingleton<IPaginationService, PaginationService>()
    .AddTransient(_ =>
    {
        var settings = builder.Configuration.GetSection("ConnectionCheck").Get<ActivityCheckSettings>();
        return new ConnectionActivityChecker(settings);
    })
    .AddHostedService<CheckService>(provider =>
    {
        var settings = builder.Configuration.GetSection("ConnectionCheck").Get<ActivityCheckSettings>();
        return new CheckService(provider.GetRequiredService<ConnectionActivityChecker>(),
            provider.GetRequiredService<IConnectionService>(), settings);
    })
    .AddSignalR();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ManipulationHub>("/manipHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();