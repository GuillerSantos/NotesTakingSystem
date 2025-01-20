namespace NTS.Client.Models.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
