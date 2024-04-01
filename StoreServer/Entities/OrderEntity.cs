namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class OrderEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public UserEntity User { get; set; }
        public int? OrderPrice { get; set; }
        public List<OrderPositionEntity> OrderPosition { get; set; }
        public string? OrderDate { get; set; }
    }
}
