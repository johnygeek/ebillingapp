using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EBillAppTest.Data;
using EBillAppTest.Helpers;

namespace EBillAppTest.Controllers
{
    public class OrderController : ApiController
    {
        private EBillEntities db = new EBillEntities();

        // GET: api/Order
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Order/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Order/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Order
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Models.OrderViewModel order)
        {
            var dbOrder = new Order();
            dbOrder.Id = Guid.NewGuid();
            dbOrder.CustomerId = order.OrderInfo.CustomerId;
            decimal amount = 0;
            foreach (var item in order.Items)
            {
                OrderItem oitem = new OrderItem();
                oitem.Id = Guid.NewGuid();
                oitem.OrderId = dbOrder.Id;
                oitem.ItemId = item.Id;
                db.OrderItems.Add(oitem);
                var itemType = db.ItemTypes.SingleOrDefault(it => it.ItemId == item.Id && it.TypeId == (int)item.Type);
                if (itemType != null)
                    amount = amount + itemType.Amount;
            }
            dbOrder.Amount = amount;
            db.Orders.Add(dbOrder);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(dbOrder.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            //Send Email
                var dbCust = db.Customers.SingleOrDefault(cust => cust.Id == dbOrder.CustomerId);
                string to = dbCust.Email;
                string from = "noreply@cravetheshake.com";
                string orderDetailsTable = TableBuilder.BuildTable(order);
                string emailMsgContent = EmailContentHelper.GenerateContent(orderDetailsTable, dbOrder.Id, dbOrder.Amount, dbCust.FirstName + " " + dbCust.LastName);
                await EmailHelper.SendEmailAsync(emailMsgContent, from, to);
                return Ok(new { status = "success" });
        }

        // DELETE: api/Order/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}