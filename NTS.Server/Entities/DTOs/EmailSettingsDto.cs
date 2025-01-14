namespace NTS.Server.Entities.DTOs
{
    public class EmailSettingsDto
    {
        public string SMTPServer { get; set; } = string.Empty;
        public int SMTPPort { get; set; }
        public string FromEmail { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
