using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StoreClient.View
{
    /// <summary>
    /// Product configurator window. Has instruments for adding new products
    /// </summary>
    public partial class ProductConfiguratorWindow : Window
    {
        public ProductConfiguratorWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button click event handler. Sends product details to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddProductButtonClickAsync(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            int price = product.TextBoxValidation(PriceBox.Text);
            if (price == 0)
            {
                MessageBox.Show("Цена должна быть больше 0");
                return;
            }

            product.Name = NameBox.Text;
            product.Price = price;
            var response = await App.client.PostAsJsonAsync("/products", product);
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                MessageBox.Show("Недостаточно прав");
            }
            this.Close();
        } 
    }
}
