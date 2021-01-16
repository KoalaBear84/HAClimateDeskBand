using Newtonsoft.Json;

namespace HAClimateDeskband.Models
{
    public class EntityModel
    {
        [JsonProperty("entity_id")]
        public string ClimateEntityId { get; set; }
    }
}
