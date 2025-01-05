using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;

namespace NTS.Client.Pages.LoginPage
{
    public class LoginBase : ComponentBase
    {
        public readonly DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        [Inject] public IAuthService authService { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public Response response { get; set; } = new Response();
        public LoginDto loginDtos = new LoginDto();
        public string responseMessage = string.Empty;

        public async Task HandleLoginClick()
        {
            var result = await authService.LoginAsync(loginDtos);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Role: {response.Role}");
                var role = result.Role;

                if (role == "Admin")
                {
                    navigationManager.NavigateTo("/adminhome");
                }
                else if (role == "User")
                {
                    navigationManager.NavigateTo("/userhome");
                }
                else
                {
                    responseMessage = "Unknown Role Detected. Please Contact Support";
                }

                StateHasChanged();
            }
            else
            {
                responseMessage = result.Message;
            }
        }


        public async Task OpenForgotPasswordDialog()
        {
            var parameters = new DialogParameters<ForgotPasswordDialog>();

            var dialog = dialogService.Show<ForgotPasswordDialog>("Forgot Password", parameters, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled && result.Data is string resetToken)
            {
                snackbar.Add($"Password Reset Email Has Been Sent To Your Email: {resetToken}", Severity.Info);
            }
        }


        public void ShowPasswordClick()
        {
            showPasswordUtil.Toggle();
        }
    }
}