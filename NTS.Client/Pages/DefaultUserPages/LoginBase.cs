using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.DTOs;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class LoginBase : ComponentBase
    {
        [Inject] public IAuthService authService { get; set; } = default!;
        [Inject] public NavigationManager navigationManager { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;
        [Inject] public ISnackbar snackbar { get; set; } = default!;
        [Inject] public ILocalStorageService localStorageService { get; set; } = default!;
        [Inject] public CustomAuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public ResponseDto responseDto = new ResponseDto();
        public LoginDto loginDto = new LoginDto();

        public readonly DialogOptions dialogOptions = new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            NoHeader = true
        };


        public async Task HandleLoginClick()
        {
            responseDto.ErrorMessage = null;

            var response = await authService.LoginAsync(loginDto);

            if (response.IsSuccess)
            {
                var token = await localStorageService.GetItemAsStringAsync("Token");
                var refreshToken = await localStorageService.GetItemAsStringAsync("RefreshToken");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
                {
                    responseDto.ErrorMessage = response.ErrorMessage;
                    return;
                }

                await authenticationStateProvider.RefreshAuthenticationStateAsync();
                navigationManager.NavigateTo("/home");
            }
            else
            {
                responseDto.ErrorMessage = response.ErrorMessage;
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
