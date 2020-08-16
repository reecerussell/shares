using System.Text.Json.Serialization;

namespace Shares.Core.Dtos
{
    public class CreateSellOrderDto
    {
        [JsonIgnore]
        public string UserId { get; set; }
        
        [JsonIgnore]
        public string OrderId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
