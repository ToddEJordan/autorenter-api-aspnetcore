namespace AutoRenter.Api
{
    public class AppSettings
    {
        public virtual bool InMemoryProvider { get; set; }

        public virtual TokenSettings TokenSettings { get; set; }
    }

    public class TokenSettings
    {
        public virtual string Issuer { get; set; }

        public virtual string Audience { get; set; }

        public virtual int ExpirationMinutes { get; set; }

        public virtual string Secret { get; set; }
    }
}
