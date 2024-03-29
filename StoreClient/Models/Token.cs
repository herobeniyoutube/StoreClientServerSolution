using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreClient.Models
{
 
    public class Token
    {

        public string accessToken {get;set;}
   
        public  string userLogin { get; set; }

        public  string userName { get; set; }

        public  string text { get; set; }

        public  int id { get; set; }

        public  string role { get; set; }
    }
}
