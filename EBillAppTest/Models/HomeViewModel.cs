using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillAppTest.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}