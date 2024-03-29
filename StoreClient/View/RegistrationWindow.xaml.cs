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
    /// Registration dialog window
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button click event handler. Sends client details to server for registration. May return 409 if user already exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterButtonClickAsync(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string name = NameBox.Text;

            if(PasswordBox.Password.Length < 8 || 16 < PasswordBox.Password.Length)
            {
                MessageBox.Show("Пароль должен быть не меньше восьми и не больше шестнадцати символов");
                return;
            }

            byte[] data = Encoding.ASCII.GetBytes(PasswordBox.Password);
            SHA256 hash = SHA256.Create();
            string passwordEncrypted = Convert.ToHexString(hash.ComputeHash(data));

            User user = new User() { UserLogin = login, UserName = name, Password = passwordEncrypted };

            var response = await App.client.PostAsJsonAsync("/register", user);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (responseContent == "409")
            {
                MessageBox.Show("Логин занят");
                return;
            }
            var token = JsonSerializer.Deserialize<Token>(responseContent);
            App.Token = token;
            App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", App.Token.accessToken);
            this.Close();
        }
    }
}
