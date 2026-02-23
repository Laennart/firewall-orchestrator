using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace FWO.Data
{
    public class OwnerIdModel
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
