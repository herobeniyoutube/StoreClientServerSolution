using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreClients
{
    public class Users
    {

        public int Id { get; set; }

        public string? UserName { get; set; }
        public string? UserLogin { get; set; }
    }
}
