using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Entities
{
    /// <summary>
    /// Ef core entity 
    /// </summary>
    public class UserEntity 
    {

        public int Id { get; set; }

        public string? UserName { get; set; }
        public string? UserLogin { get; set; }
        public List<OrderEntity> Orders { get; set;}

        public string? Password { get; set; }
    }
}
