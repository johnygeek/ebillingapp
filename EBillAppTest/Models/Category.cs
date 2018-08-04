using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillAppTest.Models
{
    public class Category
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}