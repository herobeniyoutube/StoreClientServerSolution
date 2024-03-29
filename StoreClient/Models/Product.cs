using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


public class Product
{
    public Product() { }
    public Product(string productName, int price)
    {
        ProductName = productName;
        Price = price;
    }
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int? Price { get; set; }
    public override string ToString() => $"{ProductName}";
    /// <summary>
    /// Button click event handler. Makes sure that input is not 0 or not int 
    /// </summary>
    public int TextBoxValidation(string input)
    {
        int result = 0;
        try
        {
            result = Convert.ToInt32(input);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }
}

