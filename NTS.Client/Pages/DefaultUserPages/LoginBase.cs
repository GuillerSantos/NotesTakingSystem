﻿using Blazored.LocalStorage;
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
        #region Fields

        public readonly DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            NoHeader = true
        };

        public ShowPasswordUtil showPasswordUtil = new();
        public ResponseDto responseDto = new();
        public LoginDto loginDto = new();

        #endregion Fields

        #region Properties

        [Inject] public IAuthService authService { get; set; } = default!;
        [Inject] public NavigationManager navigationManager { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;
        [Inject] public ISnackbar snackbar { get; set; } = default!;
        [Inject] public ILocalStorageService localStorageService { get; set; } = default!;
        [Inject] public CustomAuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

        #endregion Properties

        #region Public Methods

        public async Task HandleLoginClick()
        {
            responseDto.ErrorMessage = null;

            var response = await authService.LoginAsync(loginDto);
            if (!response.IsSuccess)
            {
                responseDto.ErrorMessage = response.ErrorMessage;
                return;
            }

            await authenticationStateProvider.RefreshAuthenticationStateAsync();
            navigationManager.NavigateTo("/home");
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

        #endregion Public Methods
    }
}