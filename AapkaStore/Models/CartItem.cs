using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AapkaStore.Models
{
    public class CartItem
    {
        public Item item {get; set;}
        public int quantity { get; set; }
    }
}