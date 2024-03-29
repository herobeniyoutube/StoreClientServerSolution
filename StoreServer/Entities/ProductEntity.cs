namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class ProductEntity
    {
        public int Id { get; set; } 
        public string? ProductName { get; set; }
        public int? Price { get; set;}
    }
}
