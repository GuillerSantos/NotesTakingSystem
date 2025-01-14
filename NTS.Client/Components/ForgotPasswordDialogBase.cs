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
        [Parameter] public ForgotPasswordDto forgotPasswordDto { get; set; } = new ForgotPasswordDto();
        [Inject] IAuthService authService { get; set; }

        public ResponseDto response { get; set; } = new ResponseDto();
        public string ResetToken { get; set; }

        public async Task Submit()
        {
            try
            {
                var result = await authService.ForgotPasswordAsync(forgotPasswordDto);

                if (!string.IsNullOrWhiteSpace(response?.Token))
                {
                    ResetToken = response.Token;
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