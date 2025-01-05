namespace NTS.Client.Domain.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
