using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;
using System.Security.Claims;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.UserPages
{
    public class LoginBase : ComponentBase
    {
        [Inject] public IAuthService authService { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public ILocalStorageService localStorageService { get; set; }
        [Inject] public CustomAuthenticationStateProvider authenticationStateProvider { get; set; }

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public string? errorMessage;
        public LoginDto loginDto { get; set; } = new LoginDto();

        public readonly DialogOptions dialogOptions = new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            NoHeader = true
        };

        public async Task HandleLoginClick()
        {
            errorMessage = null;
            try
            {
                var response = await authService.LoginAsync(loginDto);

                if (response.IsSuccess)
                {
                    var token = await localStorageService.GetItemAsStringAsync("Token");
                    if (!string.IsNullOrEmpty(token))
                    {
                        navigationManager.NavigateTo("/userdashboard");
                    }
                    else
                    {
                        Console.WriteLine("Login successful, But Token Not Found");
                    }
                }
                else
                {
                    errorMessage = response.ErrorMessage;
                }
            }
            catch (Exception error)
            {
                throw new Exception($"An error occurred while logging in: {error.Message}");
            }
        }

        public async Task OpenForgotPasswordDialog()
        {
            var parameters = new DialogParameters<ForgotPasswordDialog>();

            var dialog = dialogService.Show<ForgotPasswordDialog>("Forgot Password", parameters, dialogOptions);

            var result = await dialog.Result;

            if (result!.Canceled && result.Data is string resetToken)
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
