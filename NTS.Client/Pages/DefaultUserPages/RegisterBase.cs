using Microsoft.AspNetCore.Components;
using NTS.Client.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class RegisterBase : ComponentBase
    {
        #region Fields

        public ResponseDto responseDto = new ResponseDto();

        #endregion Fields

        #region Properties

        [Inject] public IAuthService authService { get; set; }

        public RegisterDto register { get; set; } = new RegisterDto();

        #endregion Properties

        #region Public Methods

        public async Task HandleRegisterAsync()
        {
            var response = await authService.RegisterDefaultUserAsync(register);
            if (response.IsSuccess)
            {
                Console.WriteLine("Account Registered Successfully");
            }
            else
            {
                responseDto.ErrorMessage = response.ErrorMessage;
            }
        }

        #endregion Public Methods
    }
}