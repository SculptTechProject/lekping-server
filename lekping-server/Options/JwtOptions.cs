namespace lekping.server.Options
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
        public int ExpiryMinutes { get; set; } = 60;
        public double ExpireMinutes { get; internal set; }
    }
}
