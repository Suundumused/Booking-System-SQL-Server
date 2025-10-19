namespace ZConnector.Models.JWT
{
    public class CommonJwtSettings
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public byte[]? Secret { get; set; }

        public double ExpirationDate { get; set; }
    }
}
