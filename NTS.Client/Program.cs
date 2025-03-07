global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;
using NTS.Client.Services;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });

builder.Services.AddMudServices();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<HubConnection>(sp =>
new HubConnectionBuilder()
    .WithUrl("https://localhost:7172/commenthub")
    .WithAutomaticReconnect()
    .Build());

// DI Services.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IFavoriteNotesService, FavoriteNotesService>();
builder.Services.AddScoped<IImportantNotesService, ImportantNotesService>();
builder.Services.AddScoped<ISharedNotesService, SharedNotesService>();
builder.Services.AddScoped<IStarredNotesService, StarredNotesService>();
builder.Services.AddScoped<ICommentSignalRService, CommentSignalRService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ThemeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();