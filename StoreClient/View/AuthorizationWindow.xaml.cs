using StoreClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
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
using System.Windows.Shapes;

namespace StoreClient.View
{
    /// <summary>
    /// Authorization window
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button click event handler.  Sends user details to the server. Sets jwt token if user found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AuthorizeButtonAsyncClick(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;

            byte[] data = Encoding.ASCII.GetBytes("11111111"); //PasswordBox.Password 
            SHA256 hash = SHA256.Create();
            string passwordEncrypted = Convert.ToHexString(hash.ComputeHash(data));

            User user = new User() { UserLogin = login, Password = passwordEncrypted };
            var response = await App.client.PostAsJsonAsync("/login", user).Result.Content.ReadAsStringAsync();
            if (response == "404")
            {
                MessageBox.Show("Пользователя не существует");
                return;
            }
            else if (response == "406")
            {
                MessageBox.Show("Неверный пароль");
                return;
            }
            var token = JsonSerializer.Deserialize<Token>(response);
            App.Token = token;
            App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", App.Token.accessToken);
            this.Close();
        }
    }
}
