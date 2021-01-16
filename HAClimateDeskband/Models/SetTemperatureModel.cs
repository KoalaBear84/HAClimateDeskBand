using Newtonsoft.Json;

namespace HAClimateDeskband.Models
{
    public class SetTemperatureModel : EntityModel
    {
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
    }
}
