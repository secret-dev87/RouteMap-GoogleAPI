namespace MapRoute.Models
{
    internal class Address
    {
        public Int64 ID { get; set; }
        public string? State { get; set; }
        public string? CountryCode { get; set; }
        public string? ResidenceCity { get; set; }
        public string? ResidenceState { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string? CleanAddress { get; set; }
        public int WalkOrder { get; set; }
    }
}
