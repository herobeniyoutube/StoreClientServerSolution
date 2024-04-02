using System.ComponentModel.DataAnnotations;

namespace StoreServer.Models.EFCoreEntitiesCopies
{

    public class Product
    {
        public int Id { get; set; }
         
        public string? Name { get; set; }
        public int? Price { get; set; }
    }
}
