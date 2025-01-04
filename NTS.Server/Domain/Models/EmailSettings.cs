namespace NTS.Server.Domain.Models
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string FromEmail { get; set; }
        public string AppPassword { get; set; }
        public string FromName { get; set; }
    }
}
