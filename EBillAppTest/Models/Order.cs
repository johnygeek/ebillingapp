using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBillAppTest.Data;

namespace EBillAppTest.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public Order OrderInfo { get; set; }
    }
}