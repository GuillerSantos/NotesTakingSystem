using Microsoft.AspNetCore.Components;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.LoginPage
{
    public class LoginBase : ComponentBase
    {
        [Inject] public IAuthService authService { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }

        public LoginDtos loginDtos = new LoginDtos();
        public string responseMessage = string.Empty;
        public bool isSuccess;

        public async Task LoginAsync()
        {
            var response = await authService.LoginAsync(loginDtos);
            if (response.IsSuccess)
            {
                navigationManager.NavigateTo("/dashboard");
                StateHasChanged();
            }
            else
            {
                responseMessage = response.Message;
            }
        }
    }
}