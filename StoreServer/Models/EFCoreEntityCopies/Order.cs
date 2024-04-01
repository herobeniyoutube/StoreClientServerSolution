namespace StoreServer.Models.EFCoreEntitiesCopies
{

    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int? OrderPrice { get; set; }
        public List<OrderPosition> OrderPosition { get; set; }
        public string OrderDate { get; set; }
    }
}
