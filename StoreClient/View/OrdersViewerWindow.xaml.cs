using Azure;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreClient.View
{
    /// <summary>
    /// Order viewer window. Has instruments for managing existing orders. Editing,deletion
    /// </summary>
    public partial class OrdersViewerWindow : Window
    {  
        List<Order>? orders;
        Order currentOrder;

        public OrdersViewerWindow()
        {
            InitializeComponent();

            this.Loaded += GetOrdersAsync;

            OrdersComboBox.SelectionChanged += ChooseOrderOnSelectionChanged;
        }
        /// <summary>
        /// Gets all orders of current User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetOrdersAsync(object sender, RoutedEventArgs e)
        {
            if (orders is null) orders = await App.client.GetFromJsonAsync<List<Order>>($"/users/{App.Token.id}/orders");

            OrdersComboBox.ItemsSource = orders;
        }
        /// <summary>
        /// Gets all positions for current Order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task<List<OrderPosition>> GetOrderPositionsAsync(object sender, RoutedEventArgs e)
        {
            var response = await App.client.GetFromJsonAsync<List<OrderPosition>>($"/users/{App.Token.id}/orders/{currentOrder.Id}/position");

            return response;
        }
        /// <summary>
        /// On change getting positions for chosen order. Shows it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChooseOrderOnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (OrdersComboBox.SelectedValue is null) return;

            //Gets order id from combobox value name
            string comboBoxOrderName = OrdersComboBox.SelectedValue.ToString();
            int i = comboBoxOrderName.IndexOf('d') + 3;
            int id = Convert.ToInt32(comboBoxOrderName.Substring(i));

            currentOrder = orders.FirstOrDefault(x => x.Id == id);
            
            ShowOrder();
        }
        /// <summary>
        /// Shows order 
        /// </summary>
        private void ShowOrder()
        {
            ChosenOrderView.Children.Clear();
            if (currentOrder is null) { return; }
          
            foreach (var position in currentOrder.Positions)
            {
                ChosenOrderView.Children.Add(new CheckBox
                {
                    Content = $"Название: {position.Product.Name} " +
                    $"Количество: {position.ProductQuantity} " +
                    $"Цена: {position.ProductQuantity * position.Product.Price}",
                    IsChecked = false,
                    Name = $"Id{position.Product.Id}"
                });
            }
        }
        /// <summary>
        /// Deletes position from order. Does not save changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePositionButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить позиции?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            if (currentOrder.Positions.Count == 0) return;

            CheckBox checkBox = new CheckBox();
            OrderPosition positionForDeletion;
            List<OrderPosition> tempDeletedPositionsList = new List<OrderPosition>();

            foreach (var item in ChosenOrderView.Children)
            {
                checkBox = item as CheckBox;
                if ((bool)checkBox.IsChecked)
                {
                    positionForDeletion = currentOrder.Positions.FirstOrDefault(x => x.ProductId == Convert.ToInt32(checkBox.Name.Substring(2)));
                    currentOrder.Positions.Remove(positionForDeletion);
                }
            }

            currentOrder.Price = currentOrder.Positions.Sum((x) => x.ProductQuantity * x.Product.Price);

            OrdersComboBox.ItemsSource = orders;

            ShowOrder();
        }
        /// <summary>
        /// Updates order on the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateOrderButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (currentOrder is null) return; 
            if (currentOrder.Price == 0)
            {
                MessageBox.Show("Пустой заказ");
                return;
            }

            var updateResponse = await App.client.PutAsJsonAsync($"/users/{App.Token.id}/orders/{currentOrder.Id}", currentOrder);
            orders = await App.client.GetFromJsonAsync<List<Order>>($"/users/{App.Token.id}/orders");
            GetOrdersAsync(sender, e);
        }
        /// <summary>
        /// Deletes order from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteOrderButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (currentOrder is null) return;

            var result = MessageBox.Show("Удалить заказ?", "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return;

            var orderDeletionResponse = await App.client.DeleteFromJsonAsync<Order>($"/users/{App.Token.id}/orders/{currentOrder.Id}");

            orders = await App.client.GetFromJsonAsync<List<Order>>($"/users/{App.Token.id}/orders");
            currentOrder = null;
            GetOrdersAsync(sender, e);
            ShowOrder();
        }
    }
}
