using EBillAppTest.Models;
using EBillAppTest.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace EBillAppTest.Helpers
{
    public class TableBuilder
    {
        static EBillEntities db = new EBillEntities();
        public static string BuildTable(OrderViewModel order)
        {
            string outerTable = "<table><tr><th>Item</th><th>Type</th><th>Amount</th></tr><tbody>{0}</tbody></table>";
            List<string> rows = new List<string>();
            foreach (var item in order.Items)
            {
                var dbItemType = db.ItemTypes.SingleOrDefault(it => it.ItemId == item.Id && it.TypeId == (int)item.Type);
                var dbItem = db.Items.SingleOrDefault(it => it.Id == item.Id);
                rows.Add("<tr><td>" + dbItem.Name + "</td><td>" + dbItemType.Type.Name + "</td><td>" + string.Format(new CultureInfo("en-IN"),"{0:C}", dbItemType.Amount) + "</td></tr>");
            }
            return string.Format(outerTable, string.Join(string.Empty, rows));
        }
    }
}