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
using StoreClient;

using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        private static string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
        private static IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(applicationPath)
                .AddJsonFile("clientsettings.json", optional: false, reloadOnChange: true)
                .Build();

        private static Uri uri = new Uri($"{configurationRoot["ServerUrl"]}");
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
