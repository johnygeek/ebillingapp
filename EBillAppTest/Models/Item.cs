using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillAppTest.Models
{
    public class Item
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public ItemType Type { get; set; }

        public int Quantity { get; set; }
    }
}