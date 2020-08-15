namespace Shares.Core.Dtos
{
    public class CreateSellOrderDto
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
