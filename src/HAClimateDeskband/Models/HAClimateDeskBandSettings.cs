namespace HAClimateDeskband.Models
{
    public class HAClimateDeskBandSettings
    {
        public string ApiBaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string ClimateEntityId { get; set; }
        public string TemperatureEntityId { get; set; }
        public string PowerUsageEntityId { get; set; }
        public bool PreferLastChangeAndPowerUsage { get; set; }
    }
}
