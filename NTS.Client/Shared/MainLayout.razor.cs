using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using NTS.Client.DTOs;
using NTS.Client.Services;
using YourApp.Client.Securities;

namespace NTS.Client.Shared
{
    public partial class MainLayout
    {
        public bool drawerOpen = false;

        [Inject] NavigationManager navigationManager { get; set; } = default!;
        [Inject] ILocalStorageService localStorageService { get; set; } = default!;
        [Inject] CustomAuthenticationStateProvider authenticationState { get; set; } = default!;
        [Inject] ThemeService themeService { get; set; } = default!;

        public ResponseTokenDto responseToken = new ResponseTokenDto();
        private bool themeLoaded = false;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !themeLoaded)
            {
                await themeService.OnInitializedAsync();
                themeLoaded = true;
                StateHasChanged();
            }
        }
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