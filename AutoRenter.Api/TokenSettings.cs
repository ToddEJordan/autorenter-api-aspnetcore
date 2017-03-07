namespace AutoRenter.Api
{
    public class TokenSettings
    {
        public virtual string Issuer { get; set; }

        public virtual string Audience { get; set; }

        public virtual int ExpirationMinutes { get; set; }

        public virtual string Secret { get; set; }
    }
}
