using EBillAppTest.Data;
using EBillAppTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace EBillAppTest.Helpers
{
    public class TableBuilder
    {
        private static EBillEntities db = new EBillEntities();

        public static string BuildTable(OrderViewModel order)
        {
            string format = "<table><tr><th align='left'>Item</th><th align='left'>Type</th><th align='center'>Quantity</th><th align='right'>Amount</th></tr><tbody>{0}</tbody></table>";
            List<string> stringList = new List<string>();
            Decimal num = new Decimal();
            foreach (EBillAppTest.Models.Item obj1 in order.Items)
            {
                EBillAppTest.Models.Item item = obj1;
                var itemType = db.ItemTypes.SingleOrDefault(it => it.ItemId == item.Id && it.TypeId == (int)item.Type);
                EBillAppTest.Data.Item obj2 = TableBuilder.db.Items.SingleOrDefault<EBillAppTest.Data.Item>((Expression<Func<EBillAppTest.Data.Item, bool>>)(it => it.Id == item.Id));
                stringList.Add("<tr><td style='width: 50%'>" + obj2.Name + "</td><td style='width: 10%'>" + itemType.Type.Name + "</td><td style='width: 10%' align='center'>" + (object)item.Quantity + "</td><td align='right' style='width: 30%'>" + string.Format((IFormatProvider)new CultureInfo("en-IN"), "{0:C}", (object)(itemType.Amount * (Decimal)item.Quantity)) + "</td></tr>");
                num += itemType.Amount * (Decimal)item.Quantity;
            }
            stringList.Add("<tr><td colspan='4' /></tr><tr><td colspan='4' /></tr><tr><td colspan='3' style='width: 100%'><strong>Total Amount</strong></td><td align='right'>" + string.Format((IFormatProvider)new CultureInfo("en-IN"), "{0:C}", (object)num) + "</td></tr>");
            return string.Format(format, (object)string.Join(string.Empty, (IEnumerable<string>)stringList));
        }
    }
}