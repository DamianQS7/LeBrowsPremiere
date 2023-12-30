namespace LeBrowsPremiere.Models
{
    public class EmailConfigurationModel
    {
        public string SmtpClientHost { get; set; }
        public int SmtpClientPort { get; set; }
        public string NetworkCredentialEmail { get; set; }
        public string NetworkCredentialPassword { get; set; }
    }
}
