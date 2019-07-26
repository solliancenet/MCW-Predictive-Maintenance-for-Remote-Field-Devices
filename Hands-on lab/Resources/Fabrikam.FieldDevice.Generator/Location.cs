using Newtonsoft.Json;

namespace Fabrikam.FieldDevice.Generator
{
    public class Location
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }

        public Location(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }
}
