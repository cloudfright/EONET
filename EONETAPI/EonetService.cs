using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Linq;

// API docs https://eonet.sci.gsfc.nasa.gov/docs/v3
// HOW to guide https://eonet.sci.gsfc.nasa.gov/how-to-guide

namespace EonetApi
{
	public class EonetService
	{
		private static readonly HttpClient client = new HttpClient();
		public static readonly Dictionary<string, string> Categories = new Dictionary<string, string>
		{
			{ "Drought","drought" },
			{ "Dust and Haze", "dustHaze" },
			{ "Earthquakes", "earthquakes" },
			{ "Floods","floods" },
			{ "Landslides","landslides" },
			{ "Manmade","manmade" },
			{ "Sea and Lake Ice","seaLakeIce" },
			{ "Severe Storms","severeStorms" },
			{ "Snow","snow" },
			{ "Temperature Extremes","tempExtremes" },
			{ "Volcanoes","volcanoes" },
			{ "Water Color", "waterColor" },
			{ "Wildfires","wildfires" }
		};

		public static readonly List<int> MaxResults = new List<int> { 10, 25, 50 };

		public async Task<EonetEvents> GetEvents(List<string>categories, int numResults)
		{
			client.DefaultRequestHeaders.Accept.Clear();

			client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

			string categoryParameters = "";
			string lastItem = categories.Last();
			foreach (var category in categories)
            {
				string categoryId;
				if (Categories.TryGetValue(category, out categoryId))
				{ 
					categoryParameters += categoryId;
					if (!category.Equals(lastItem))
					{
						categoryParameters += ",";
					}
				}			
			}

			string query = String.Format("https://eonet.sci.gsfc.nasa.gov/api/v3/events?status=open&category={0}&limit={1}", categoryParameters, numResults);
			var streamTask = client.GetStreamAsync(query);
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


/*


	"title": "EONET Event Categories",
	"description": "List of all the available event categories in the EONET system",
	"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories",
	"categories": [
		{
			"id": "drought",
			"title": "Drought",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/drought",
			"description": "Long lasting absence of precipitation affecting agriculture and livestock, and the overall availability of food and water.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/drought"
		},
		{
			"id": "dustHaze",
			"title": "Dust and Haze",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/dustHaze",
			"description": "Related to dust storms, air pollution and other non-volcanic aerosols. Volcano-related plumes shall be included with the originating eruption event.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/dustHaze"
		},
		{
			"id": "earthquakes",
			"title": "Earthquakes",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/earthquakes",
			"description": "Related to all manner of shaking and displacement. Certain aftermath of earthquakes may also be found under landslides and floods.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/earthquakes"
		},
		{
			"id": "floods",
			"title": "Floods",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/floods",
			"description": "Related to aspects of actual flooding--e.g., inundation, water extending beyond river and lake extents.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/floods"
		},
		{
			"id": "landslides",
			"title": "Landslides",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/landslides",
			"description": "Related to landslides and variations thereof: mudslides, avalanche.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/landslides"
		},
		{
			"id": "manmade",
			"title": "Manmade",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/manmade",
			"description": "Events that have been human-induced and are extreme in their extent.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/manmade"
		},
		{
			"id": "seaLakeIce",
			"title": "Sea and Lake Ice",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/seaLakeIce",
			"description": "Related to all ice that resides on oceans and lakes, including sea and lake ice (permanent and seasonal) and icebergs.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/seaLakeIce"
		},
		{
			"id": "severeStorms",
			"title": "Severe Storms",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/severeStorms",
			"description": "Related to the atmospheric aspect of storms (hurricanes, cyclones, tornadoes, etc.). Results of storms may be included under floods, landslides, etc.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/severeStorms"
		},
		{
			"id": "snow",
			"title": "Snow",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/snow",
			"description": "Related to snow events, particularly extreme/anomalous snowfall in either timing or extent/depth.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/snow"
		},
		{
			"id": "tempExtremes",
			"title": "Temperature Extremes",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/tempExtremes",
			"description": "Related to anomalous land temperatures, either heat or cold.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/tempExtremes"
		},
		{
			"id": "volcanoes",
			"title": "Volcanoes",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/volcanoes",
			"description": "Related to both the physical effects of an eruption (rock, ash, lava) and the atmospheric (ash and gas plumes).",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/volcanoes"
		},
		{
			"id": "waterColor",
			"title": "Water Color",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/waterColor",
			"description": "Related to events that alter the appearance of water: phytoplankton, red tide, algae, sediment, whiting, etc.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/waterColor"
		},
		{
			"id": "wildfires",
			"title": "Wildfires",
			"link": "https://eonet.sci.gsfc.nasa.gov/api/v3/categories/wildfires",
			"description": "Wildfires includes all nature of fire, including forest and plains fires, as well as urban and industrial fire events. Fires may be naturally caused or manmade.",
			"layers": "https://eonet.sci.gsfc.nasa.gov/api/v3/layers/wildfires"
		}
	]
}

 */ 