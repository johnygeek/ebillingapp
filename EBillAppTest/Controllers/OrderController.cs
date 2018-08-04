using EBillAppTest.Data;
using EBillAppTest.Helpers;
using EBillAppTest.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace EBillAppTest.Controllers
{
    public class OrderController : ApiController
    {
        private EBillEntities db = new EBillEntities();

        public IQueryable<Order> GetOrders()
        {
            return (IQueryable<Order>)this.db.Orders;
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(Guid id)
        {
            OrderController orderController = this;
            Order async = await orderController.db.Orders.FindAsync((object)id);
            return async != null ? (IHttpActionResult)orderController.Ok<Order>(async) : (IHttpActionResult)orderController.NotFound();
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(Guid id, Order order)
        {
            OrderController orderController = this;
            if (!orderController.ModelState.IsValid)
                return (IHttpActionResult)orderController.BadRequest(orderController.ModelState);
            if (id != order.Id)
                return (IHttpActionResult)orderController.BadRequest();
            orderController.db.Entry<Order>(order).State = EntityState.Modified;
            try
            {
                int num = await orderController.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!orderController.OrderExists(id))
                    return (IHttpActionResult)orderController.NotFound();
                throw;
            }
            return (IHttpActionResult)orderController.StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(OrderViewModel order)
        {
            Order dbOrder = new Order();
            dbOrder.Id = Guid.NewGuid();
            dbOrder.CustomerId = order.OrderInfo.CustomerId;
            Decimal num1 = new Decimal();
            foreach (EBillAppTest.Models.Item obj in order.Items)
            {
                EBillAppTest.Models.Item item = obj;
                db.OrderItems.Add(new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    OrderId = dbOrder.Id,
                    ItemId = item.Id,
                    Quantity = new int?(item.Quantity)
                });
                var itemType = db.ItemTypes.SingleOrDefault(it => it.ItemId == item.Id && it.TypeId == (int)item.Type);
                if (itemType != null)
                    num1 += itemType.Amount * (Decimal)item.Quantity;
            }
            dbOrder.Amount = num1;
            dbOrder.OrderNumber = "OD" + string.Format("{0:d9}", (object)(DateTime.Now.Ticks / 10L % 1000000000L));
            db.Orders.Add(dbOrder);
            try
            {
                int num2 = await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (OrderExists(dbOrder.Id))
                    return Conflict();
                throw;
            }
            var customer = db.Customers.SingleOrDefault(cust => cust.Id == dbOrder.CustomerId);
            string email = customer.Email;
            string from = "noreply@cravetheshake.com";
            await EmailHelper.SendEmailAsync(EmailContentHelper.GenerateContent(TableBuilder.BuildTable(order), dbOrder.OrderNumber, dbOrder.Amount, customer.FirstName + " " + customer.LastName), from, email, dbOrder.OrderNumber);
            return Ok(new
            {
                status = "success"
            });
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(Guid id)
        {
            OrderController orderController = this;
            Order order = await orderController.db.Orders.FindAsync((object)id);
            if (order == null)
                return (IHttpActionResult)orderController.NotFound();
            orderController.db.Orders.Remove(order);
            int num = await orderController.db.SaveChangesAsync();
            return (IHttpActionResult)orderController.Ok<Order>(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid id)
        {
            return this.db.Orders.Count<Order>((Expression<Func<Order, bool>>)(e => e.Id == id)) > 0;
        }
    }
}