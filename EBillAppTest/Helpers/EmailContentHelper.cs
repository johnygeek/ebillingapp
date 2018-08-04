using System;
using System.Globalization;

namespace EBillAppTest.Helpers
{
  public class EmailContentHelper
  {
    public static string GenerateContent(string table, string orderNumber, Decimal amount, string custName)
    {
      return string.Format("Dear {0}, <br/>Thank you for making an order# {1} of Total Amount {2} as below: <br/><br/>{3}</body></html>",      
        custName,
        orderNumber,
        string.Format(new CultureInfo("en-IN"), "{0:C}", amount),
        table
      );
    }
  }
}