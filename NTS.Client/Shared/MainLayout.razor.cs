using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Shared
{
    public partial class MainLayout
    {
        public bool drawerOpen = false;

        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] ILocalStorageService localStorageService { get; set; }
        [Inject] CustomAuthenticationStateProvider authenticationState { get; set; }

        public ResponseTokenDto responseToken { get; set; }

        public void ToggleDrawer()
        {
            this.drawerOpen = !drawerOpen;
        }

        public async Task LogoutAsync()
        {
            try
            {
                var acccessToken = await localStorageService.GetItemAsync<string>("Token");
                var refreshToken = await localStorageService.GetItemAsync<string>("RefreshToken");

                Console.WriteLine($"Access Token: {acccessToken}");
                Console.WriteLine($"Refresh Token: {refreshToken}");

                if (!string.IsNullOrEmpty(acccessToken) || !string.IsNullOrEmpty(refreshToken))
                {
                    await localStorageService.RemoveItemAsync("Token");
                    await localStorageService.RemoveItemAsync("RefreshToken");
                }

                await authenticationState.RefreshAuthenticationStateAsync();
                navigationManager.NavigateTo("/", true);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error During Logout: {error.Message}");
            }
        }
    }
}
