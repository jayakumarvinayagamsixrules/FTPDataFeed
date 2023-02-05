using Newtonsoft.Json;

namespace DataFeed.Model
{
    public class Product
    {
        public Id _id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Rating { get; set; }
    }

    public class Id
    {
        [JsonProperty("$oid")]
        public string oid { get; set; }
    }
}
