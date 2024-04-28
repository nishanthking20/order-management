using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Order_Management.Data;
using Order_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DConnection")));
// Add session
builder.Services.AddSession();
// Add email service
builder.Services.AddTransient<IEmailService, SmtpEmailService>(provider =>
{
    // Load SMTP server settings from configuration
    var smtpServer = builder.Configuration["EmailSettings:SmtpServer"];
    var smtpPort = int.Parse(builder.Configuration["EmailSettings:SmtpPort"]);
    var smtpUsername = builder.Configuration["EmailSettings:SmtpUsername"];
    var smtpPassword = builder.Configuration["EmailSettings:SmtpPassword"];


    return new SmtpEmailService(smtpServer, smtpPort, smtpUsername, smtpPassword);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
