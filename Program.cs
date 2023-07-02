
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
SessionOptions ss = new SessionOptions();
ss.Cookie = new CookieBuilder();
ss.Cookie.Name = "Session-id";
ss.Cookie.Expiration = TimeSpan.FromDays(1);

//app.UseSession( ss);
//app.UseRequestDecompression();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

string url = "https://localhost:7779";

ProcessStartInfo ps = new ProcessStartInfo();
ps.UseShellExecute = true;
ps.FileName = url;
System.Diagnostics.Process.Start(ps);

app.Run();

