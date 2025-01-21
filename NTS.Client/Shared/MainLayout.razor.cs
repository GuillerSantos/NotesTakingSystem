using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Shared
{
    public partial class MainLayout
    {
        public bool drawerOpen = false;

        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] IAuthService authService { get; set; }

        public void ToggleDrawer()
        {
            this.drawerOpen = !drawerOpen;
        }

        public async Task LogoutAsync()
        {
            await authService.LogoutAsync();
            navigationManager.NavigateTo("/", true);
        }
    }
}
