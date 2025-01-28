using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class RegisterBase : ComponentBase
    {
        [Inject] public IAuthService authService { get; set; }

        public RegisterDto register { get; set; } = new RegisterDto();
        public ResponseDto responseDto { get; set; } = new ResponseDto();
        public ApplicationUsers user;

        public async Task HandleRegisterAsync()
        {
            try
            {
                var response = await authService.RegisterDefaultUserAsync(register);
                if (response.IsSuccess)
                {
                    Console.WriteLine("Account Registered Successfully");
                }
                else
                {
                    Console.WriteLine($"Error Registering Account: {response.ErrorMessage}");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Registering Account: {error.Message}");
            }
        }
    }
}