using Microsoft.AspNetCore.Components;
using NTS.Client.Models.DTOs;
using NTS.Client.Services;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class RegisterBase : ComponentBase
    {
        [Inject] public IAuthService authService { get; set; }

        public RegisterDto register { get; set; } = new RegisterDto();

        public async Task HandleRegisterAsync()
        {
            try
            {
                var response = await authService.RegisterDefaultUserAsync(register);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Registrating Account: {error.Message}");
            }
        }
    }
}
