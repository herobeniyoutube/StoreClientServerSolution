using System.Text.Json.Serialization;

namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class OrderEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public UserEntity? User { get; set; }
        public int? Price { get; set; }
        public List<OrderPositionEntity>? Positions { get; set; }
        public DateTime? Date { get; set; }
    }
}
