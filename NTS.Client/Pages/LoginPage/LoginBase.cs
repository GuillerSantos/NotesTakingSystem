using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Securities;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;
using System.Security.Claims;

namespace NTS.Client.Pages.LoginPage
{
    public class LoginBase : ComponentBase
    {
        public readonly DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        [Inject] public IAuthService authService { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public ILocalStorageService localStorageService { get; set; }

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public Response response { get; set; } = new Response();
        public LoginDto loginDto = new LoginDto();
        public string responseMessage = string.Empty;

        public async Task HandleLoginClick()
        {
            var response = await authService.LoginAsync(loginDto);

            if (response.IsSuccess)
            {
                if (response.Role == "Admin")
                {
                    navigationManager.NavigateTo("/adminhome");
                }
                else
                {
                    navigationManager.NavigateTo("/userhome");
                }
            }
            else
            {
                snackbar.Add(response.ErrorMessage ?? "Login failed. Please try again.", Severity.Error);
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