using StoreClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Uri uri = new Uri("https://localhost:7277");
        public static Token Token {  get; set; }
        public static HttpClient client = new HttpClient() 
        {
            BaseAddress = uri
            
        };
        public static string CurrentUser
        {
            get
            {
                if (Token is null) return "Not authorized";
                else return Token.userName;
            }
        }
    }
}
