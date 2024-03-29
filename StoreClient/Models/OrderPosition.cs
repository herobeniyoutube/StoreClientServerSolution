using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OrderPosition
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int ProductQuantity { get; set; }
    /// <summary>
    /// Button click event handler.  Makes sure that input is not 0 or not int 
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