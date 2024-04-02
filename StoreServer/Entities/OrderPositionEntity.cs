using System.Text.Json.Serialization;

namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class OrderPositionEntity
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        [JsonIgnore]
        public OrderEntity Order { get; set; }
        public int? ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int? ProductQuantity { get; set; }    
    }
}
