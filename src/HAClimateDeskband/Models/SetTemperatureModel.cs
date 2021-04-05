using Newtonsoft.Json;

namespace HAClimateDeskband.Models
{
    public class SetTemperatureModel : EntityModel
    {
        [JsonProperty("temperature")]
        public decimal Temperature { get; set; }
    }
}
