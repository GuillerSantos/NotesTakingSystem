using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class ForgotPasswordDialogBase : ComponentBase
    {
        public readonly DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        [CascadingParameter] MudDialogInstance mudDialog { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        [Parameter] public ForgotPasswordDto forgotPasswordDto { get; set; } = new ForgotPasswordDto();
        [Inject] IAuthService authService { get; set; }

        public ResponseDto response { get; set; } = new ResponseDto();
        public ResponseTokenDto responseToken { get; set; } = new ResponseTokenDto();

        public async Task Submit()
        {
            try
            {
                var result = await authService.ForgotPasswordAsync(forgotPasswordDto);

                if (result.IsSuccess)
                {
                    snackbar.Add("Password Reset Email Has Been Sent", Severity.Success);
                }

                if (!string.IsNullOrWhiteSpace(responseToken?.Token))
                {
                    responseToken.ResetToken = responseToken.Token;
                    response.ErrorMessage = string.Empty;
                }
                else
                {
                    throw new Exception("Failed To Send A Password Reset Email");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
        }

        public void Cancel()
        {
            mudDialog.Cancel();
        }
    }
}