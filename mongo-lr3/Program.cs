using mongo_lr3;
using mongo_lr3.Repositories;
using mongo_lr3.Repositories.Interfaces;
using mongo_lr3.Services;
using mongo_lr3.Services.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("MongoDb"));
var client = new MongoClient(builder.Configuration["MongoDb:ConnectionString"]);
var database = client.GetDatabase(builder.Configuration["MongoDb:DbName"]);
IGridFSBucket gridFS = new GridFSBucket(database);
builder.Services.AddSingleton<IMongoDatabase>(database);
builder.Services.AddSingleton<IGridFSBucket>(gridFS);

builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IFileService, FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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
