namespace NTS.Client.DTOs
{
    public class ResponseDto
    {
        #region Properties

        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        #endregion Properties
    }
}