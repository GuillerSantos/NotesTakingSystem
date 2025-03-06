using Microsoft.JSInterop;
using MudBlazor;

namespace NTS.Client.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime jSRuntime;
        private readonly ILogger<ThemeService> logger;

        public bool isDarkMode;
        public string GetModeText() => isDarkMode ? "Light Mode" : "Dark Mode";

        public ThemeService(IJSRuntime jSRuntime, ILogger<ThemeService> logger)
        {
            this.jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task OnInitializedAsync()
        {
            var storedTheme = await jSRuntime.InvokeAsync<string>("localThemeFunction.getTheme");
            isDarkMode = storedTheme == "true";
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
                Primary = Colors.Blue.Darken2,
                Secondary = Colors.BlueGray.Darken1
            },

            PaletteDark = new PaletteDark()
            {
                Primary = Colors.LightBlue.Darken1,
                Secondary = Colors.DeepPurple.Darken1
            }
        };
    }
}