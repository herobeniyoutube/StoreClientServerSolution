using System.Text.Json.Serialization;

namespace StoreServer.Models.EFCoreEntitiesCopies
{

    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public int? Price { get; set; }
        public List<OrderPosition> Positions { get; set; }
        public DateTime? Date { get; set; }
    }
}
