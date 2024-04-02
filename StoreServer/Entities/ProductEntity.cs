using System.ComponentModel.DataAnnotations;

namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class ProductEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public int? Price { get; set;}
    }
}
