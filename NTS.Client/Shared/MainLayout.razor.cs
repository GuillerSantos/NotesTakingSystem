using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using NTS.Client.DTOs;
using NTS.Client.Services;
using YourApp.Client.Securities;

namespace NTS.Client.Shared
{
    public partial class MainLayout
    {
        #region Fields

        public bool drawerOpen = false;

        public ResponseTokenDto responseToken = new ResponseTokenDto();
        private bool themeLoaded = false;

        #endregion Fields

        #region Properties

        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private ILocalStorageService localStorageService { get; set; } = default!;
        [Inject] private CustomAuthenticationStateProvider authenticationState { get; set; } = default!;
        [Inject] private ThemeService themeService { get; set; } = default!;

        #endregion Properties

        #region Public Methods

        public void ToggleDrawer()
        {
            this.drawerOpen = !drawerOpen;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await localStorageService.RemoveItemAsync("Token");
                await localStorageService.RemoveItemAsync("RefreshToken");
                await authenticationState.RefreshAuthenticationStateAsync();
                navigationManager.NavigateTo("/");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error During Logout: {error.Message}");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !themeLoaded)
            {
                await themeService.OnInitializedAsync();
                themeLoaded = true;
                StateHasChanged();
            }
        }

        #endregion Protected Methods
    }
}