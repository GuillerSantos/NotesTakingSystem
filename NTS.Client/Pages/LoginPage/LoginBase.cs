using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.LoginPage
{
    public class LoginBase : ComponentBase
    {
        public readonly DialogOptions dialogOptions = new DialogOptions() 
        {
            MaxWidth = MaxWidth.Medium, 
            FullWidth = true, 
            NoHeader = true 
        };

        [Inject] public IAuthService authService { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public ILocalStorageService localStorageService { get; set; }
        [Inject] public CustomAuthenticationStateProvider authenticationStateProvider { get; set; }

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public Response response { get; set; } = new Response();
        public LoginDto loginDto = new();

        public async Task HandleLoginClick()
        {
            try
            {
                var response = await authService.LoginAsync(loginDto);

                if (response)
                {
                    var role = await localStorageService.GetItemAsync<string>("Role");

                    if (role == "Admin")
                    {
                        navigationManager.NavigateTo("/adminhome");
                    }
                    else if (role == "User")
                    {
                        navigationManager.NavigateTo("/userhome");
                    }
                }
                else
                {
                    snackbar.Add("Invalid email or password", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add($"An error occurred while logging in: {ex.Message}", Severity.Error);
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