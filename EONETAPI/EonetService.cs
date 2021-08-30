using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

// API docs https://eonet.sci.gsfc.nasa.gov/docs/v3
// HOW to guide https://eonet.sci.gsfc.nasa.gov/how-to-guide

namespace EonetApi
{
    public class EonetService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<EonetEvents> GetEvents()
        {
            client.DefaultRequestHeaders.Accept.Clear();
        
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync("https://eonet.sci.gsfc.nasa.gov/api/v3/events?status=open&limit20");
            var events = await JsonSerializer.DeserializeAsync<EonetEvents>(await streamTask);

            return events;
        }
    }

    public class EonetEvents
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("events")]
        public IList<Event> Events { get; set; }
    }

    public class Event
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("closed")]
        public DateTime? Closed { get; set; }

        [JsonPropertyName("categories")]
        public IList<Category> Categories { get; set; }

        [JsonPropertyName("sources")]
        public IList<Source> Sources { get; set; }

        [JsonPropertyName("geometry")]
        public IList<Geometry> Geometries { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class Source
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("magnitudeValue")]
        public double? MagnitudeValue { get; set; }

        [JsonPropertyName("magnitudeUnit")]
        public string MagnitudeUnit { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

  //      [JsonPropertyName("coordinates")]
  //      public double[] Coordinates { get; set; }
    }

    public class Coordinate
    {
        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }
    }
}
