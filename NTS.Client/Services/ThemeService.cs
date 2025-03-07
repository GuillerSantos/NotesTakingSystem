using Microsoft.JSInterop;
using MudBlazor;

namespace NTS.Client.Services
{
    public class ThemeService
    {
        #region Fields

        public bool isDarkMode;
        private readonly IJSRuntime jSRuntime;
        private readonly ILogger<ThemeService> logger;

        #endregion Fields

        #region Public Constructors

        public ThemeService(IJSRuntime jSRuntime, ILogger<ThemeService> logger)
        {
            this.jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Properties

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

        #endregion Properties

        #region Public Methods

        public string GetModeText() => isDarkMode ? "Light Mode" : "Dark Mode";

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

        #endregion Public Methods
    }
}