namespace NTS.Server.DTOs
{
    public class ResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = string.Empty;
    }
}
