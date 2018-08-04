using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillAppTest.Helpers
{
    public class EmailContentHelper
    {
        public static string GenerateContent(string table, Guid orderId, decimal amount, string custName)
        {
            string contentTemplate = "Dear {0}, thank you for making an order# {1} of Total Amount {2} as below <br/>{3}";
            return string.Format(contentTemplate, custName, orderId, amount, table);
        }
    }
}