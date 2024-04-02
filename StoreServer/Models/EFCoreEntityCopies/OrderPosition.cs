using System.Text.Json.Serialization;

namespace StoreServer.Models.EFCoreEntitiesCopies
{

    public class OrderPosition
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? ProductQuantity { get; set; }
    }
}
