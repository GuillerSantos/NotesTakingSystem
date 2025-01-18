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
        public ResponseDto responseDto { get; set; } = new ResponseDto();
        public LoginDto loginDto { get; set; } = new LoginDto();

        public readonly DialogOptions dialogOptions = new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            NoHeader = true
        };

        public async Task HandleLoginClick()
        {
            try
            {
                var response = await authService.LoginAsync(loginDto);

                if (response)
                {
                    var token = await localStorageService.GetItemAsStringAsync("Token");
                    if (!string.IsNullOrEmpty(token))
                    {
                        var claims = CustomAuthenticationStateProvider.ParseClaimsFromJwt(token);
                        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                        if (roleClaim?.Value == "Admin")
                        {
                            navigationManager.NavigateTo("/admindashboard");
                        }
                        else
                        {
                            navigationManager.NavigateTo("/userdashboard");
                        }
                    }
                    else
                    {
                        snackbar.Add("Token not found.", Severity.Error);
                    }
                }
                else
                {
                    snackbar.Add("Invalid credentials.", Severity.Error);
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
