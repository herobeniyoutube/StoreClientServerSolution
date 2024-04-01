
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

namespace StoreClient
{
    /// <summary>
    /// Окно входа/регистрации
    /// </summary>
    public partial class MainWindow : Window
    {
         
        public MainWindow()
        {
            InitializeComponent();


        }
        /// <summary>
        /// Button click event handler. Opens authorization dialog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.ShowDialog();
            if (App.Token is null)
            {
                MessageBox.Show("Ошибка входа");
                return;
            }
            currentUser.Text = App.Token.userName;
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Show();
            this.Close();
        }
        /// <summary>
        /// Button click event handler. Opens registration dialog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
            if (App.Token is null) 
            {
                MessageBox.Show("Ошибка регистрации");
                return; 
            }
            currentUser.Text = App.Token.userName;
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Show();
            this.Close();
        }

    }


}
