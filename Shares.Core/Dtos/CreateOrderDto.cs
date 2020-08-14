namespace Shares.Core.Dtos
{
    public class CreateOrderDto
    {
        public string InstrumentId { get; set; }
        public string UserId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
