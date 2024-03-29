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
    /// Order creation window
    /// </summary>
    public partial class OrderConfiguratorWindow : Window
    {
        List<Product>? productsList = App.client.GetFromJsonAsync<List<Product>>("/products").Result;
        List<OrderPosition> orderPositions = new List<OrderPosition>();
        Order currentOrder;

        public OrderConfiguratorWindow()
        {
            InitializeComponent();
            UserLogin.Text = $"{UserLogin.Text}{App.Token.userLogin}";
            ProductsComboBox.ItemsSource = productsList;
            ProductsComboBox.SelectedIndex = 1;
            this.Loaded += RefreshOrder;
        }

        /// <summary>
        /// Creates order. Sends it to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeOrderButtonClick(object sender, RoutedEventArgs e)
        {
            currentOrder.UserId = App.Token.id;
            currentOrder.OrderPrice = currentOrder.CountPrice(currentOrder.OrderPosition, productsList);
            App.client.PostAsJsonAsync($"/users/{App.Token.id}/orders", currentOrder);
            currentOrder = null;
            RefreshOrder(sender, e);
        }
        /// <summary>
        /// Updates order details on the Window
        /// </summary>
        private void RefreshOrder(object sender, RoutedEventArgs e)
        {
            PriceText.Text = "Стоимость заказа:";
            if (currentOrder is null) PriceText.Text = $"Стоимость заказа: 0";
            else PriceText.Text = $"Стоимость заказа:{currentOrder.CountPrice(currentOrder.OrderPosition, productsList)}";
            ShowOrder();
        }
        /// <summary>
        /// Show current order on the Window
        /// </summary>
        private void ShowOrder()
        {
            ScrollViewerPanel.Children.Clear();
            if (currentOrder is null) { return; }
            Product currentProduct;
            foreach (var position in currentOrder.OrderPosition)
            {
                currentProduct = productsList.FirstOrDefault(x => x.Id == position.ProductId);
                if (currentProduct is null) { return; }
                ScrollViewerPanel.Children.Add(new CheckBox
                {
                    Content = $"Название: {currentProduct.ProductName} " +
                    $"Количество: {position.ProductQuantity} " +
                    $"Цена: {position.ProductQuantity * currentProduct.Price}",
                    IsChecked = false,
                    Name = $"Id{currentProduct.Id}"
                });
            }
        }
        
        /// <summary>
        /// Adds new position to the current order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPositionButtonClick(object sender, RoutedEventArgs e)
        {
            OrderPosition? orderPosition = new OrderPosition();
            int quantity = orderPosition.TextBoxValidation(ProductQuantity.Text);
            if (quantity == 0)
            {
                MessageBox.Show("Введите числовое, целое значение больше нуля!");
                return;
            }

            string productName = ProductsComboBox.SelectedValue.ToString();
            var product = productsList.FirstOrDefault(x => x.ProductName == productName);  

            orderPosition = orderPositions.FirstOrDefault(x => x.ProductId == product.Id);

            if (orderPositions.Count > 0 && orderPosition is not null)
            {
                orderPosition.ProductQuantity += quantity;
            }
            else
            {
                orderPosition = new OrderPosition()
                {
                    ProductId = product.Id,
                    ProductQuantity = quantity
                };
                if (currentOrder is null) currentOrder = new Order() { OrderPosition = orderPositions };
                currentOrder.OrderPosition.Add(orderPosition);
            }
            RefreshOrder(sender, e);
        }
        /// <summary>
        /// Deletes position from current order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePositionButtonClick(object sender, RoutedEventArgs e)
        {
            if (orderPositions.Count == 0) return;
            CheckBox checkBox = new CheckBox();
            foreach (var item in ScrollViewerPanel.Children)
            {
                checkBox = item as CheckBox;
                if ((bool)checkBox.IsChecked)
                {
                    currentOrder.OrderPosition.Remove(currentOrder.OrderPosition.FirstOrDefault(x => x.ProductId == Convert.ToInt32(checkBox.Name.Substring(2))));
                }
            }
            ShowOrder();
        }
    }
}
