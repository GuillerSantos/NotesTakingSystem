using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Interfaces;

namespace NTS.Client.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime jSRuntime;

        public ThemeService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public bool isDarkMode;


        public string GetModeText() => isDarkMode ? "Light Mode" : "Dark Mode";


        public async Task OnInitializedAsync()
        {

            try
            {
                var storedTheme = await jSRuntime.InvokeAsync<string>("localThemeFunction.getTheme");
                isDarkMode = storedTheme == "true";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }


        public async Task ToggleDarkModeAsync()
        {
            isDarkMode = !isDarkMode;
            await jSRuntime.InvokeVoidAsync("localThemeFunction.setTheme", isDarkMode.ToString().ToLower());
        }

        public MudTheme CustomTheme { get; } = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.Purple.Darken1,
                Secondary = Colors.Green.Accent1
            },

            PaletteDark = new PaletteDark()
            {
                Primary = Colors.DeepPurple.Default,
                Secondary = Colors.Green.Accent4
            }
        };
    }
}
