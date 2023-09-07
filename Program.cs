using Microsoft.EntityFrameworkCore;
using static YCTest.Models.YC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//讓外部可以連接
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });

});
//多連接可方便使用
static string GetConnectionString(string dataSource, string userId, string password, string database)
    => $"Data Source={dataSource};Persist Security Info=True;User ID={userId};Password={password};TrustServerCertificate=true;Database={database}";
string commonSource1 = "juroserver.com";
string commonUserId1 = "sa";
string commonPassword1 = "Ar6437633";
//讓其他Controller可以直接使用不用每次都呼叫一次
builder.Services.AddDbContext<YCDbContext>(options =>
    options.UseSqlServer(GetConnectionString(commonSource1, commonUserId1, commonPassword1, "YC")));

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
