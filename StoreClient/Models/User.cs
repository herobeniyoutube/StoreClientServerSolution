﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class User
{

    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Login { get; set; }
    public List<Order> Orders { get; set; }
    public string? Password { get; set; }
}