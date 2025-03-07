using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class ForgotPasswordDialogBase : ComponentBase
    {
        #region Fields

        public readonly DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        #endregion Fields

        #region Properties

        [Parameter] public ForgotPasswordDto forgotPasswordDto { get; set; } = new ForgotPasswordDto();
        public ResponseDto response { get; set; } = new ResponseDto();
        public ResponseTokenDto responseToken { get; set; } = new ResponseTokenDto();
        [CascadingParameter] private MudDialogInstance mudDialog { get; set; } = default!;
        [Inject] private IAuthService authService { get; set; } = default!;
        [Inject] private ISnackbar snackbar { get; set; } = default!;

        #endregion Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}