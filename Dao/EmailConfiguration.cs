namespace apartease_backend.Dao
{
    public class EmailConfiguration
    {
        public string SenderEmail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public string SenderName { get; set; }

        public bool UseSsl { get; set; }
    }
}
