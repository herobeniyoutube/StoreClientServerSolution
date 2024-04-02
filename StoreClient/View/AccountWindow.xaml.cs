
using Azure.Core;
using StoreClient.Models;
using StoreClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;

namespace StoreClient.View
{
    /// <summary>
    /// Account window.Has instruments for managing orders. Instruments may differ due to user rights
    /// </summary>
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
            CurrentUser.Text = App.Token.userLogin;
            if (App.Token.role == "admin")
            {
                 AddNewProducts.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Open dialog window for adding new product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewProductsButtonClick(object sender, RoutedEventArgs e)
        {
            if (App.Token is null) { MessageBox.Show("Not authorized"); return; }
            else if (App.Token.role != "admin") { MessageBox.Show("Not authorized"); return; }
            ProductConfiguratorWindow productConfiguratorWindow = new ProductConfiguratorWindow();
            productConfiguratorWindow.ShowDialog();
        }
        /// <summary>
        /// Gets all products in the system. Shows those in message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetProductsListButtonClickAsync(object sender, RoutedEventArgs e)
        {
            List<Product> productsList = await App.client.GetFromJsonAsync<List<Product>>("/products");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Product product in productsList)
            {
                stringBuilder.AppendLine($"{product.Name}");
            }
            MessageBox.Show(stringBuilder.ToString());
        }
        /// <summary>
        /// Exist. Erases current user/jwt token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            App.client.DefaultRequestHeaders.Clear();
            App.Token = null;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        /// <summary>
        /// Opens order configurator window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeOrderButtonClick(object sender, RoutedEventArgs e)
        {
            if (App.Token is null) { MessageBox.Show("Not authorized"); return; }
            OrderConfiguratorWindow orderConfiguratorWindow = new OrderConfiguratorWindow();
            orderConfiguratorWindow.ShowDialog();
        }
        private void OrdersViewerOpenButton(object sender, RoutedEventArgs e)
        {
            if (App.Token is null) { MessageBox.Show("Not authorized"); return; }
            OrdersViewerWindow ordersViewerWindow = new OrdersViewerWindow();
            ordersViewerWindow.ShowDialog();
        }
        /// <summary>
        /// Downloads json file which contains all user's orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DownloadOrdersButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var orders = await App.client.GetAsync($"/users/{App.Token.id}/orders").Result.Content.ReadAsStringAsync();
            string ordersJson = JsonSerializer.Serialize(orders);
            await File.AppendAllTextAsync($"Orders_{App.Token.userLogin}.json", ordersJson);
        }
    }
}
