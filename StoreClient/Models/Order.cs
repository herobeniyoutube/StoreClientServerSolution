using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
    public int? OrderPrice { get; set; }
    public List<OrderPosition> OrderPosition { get; set; }
    public override string ToString() => $"Стоимость заказа: {OrderPrice} Id: {Id}";
    /// <summary>
    /// Count order's price
    /// </summary>
    public int CountPrice(List<OrderPosition> orderPositions, List<Product> productsList)
    {
        int result = 0;
        if (productsList == null) return result;
        foreach (var item in orderPositions)
        {
            result += (int)(item.ProductQuantity * productsList.First(x => x.Id == item.ProductId).Price);
        }
        return result;
    }
}

