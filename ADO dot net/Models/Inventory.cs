﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADO_dot_net.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedOn { get; set; }
    }
}