using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EBillAppTest.Data;

namespace EBillAppTest.Controllers
{
    public class CustomerController : ApiController
    {
        private EBillEntities db = new EBillEntities();

        // GET: api/Customer
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/Customer/5
        [ResponseType(typeof(Models.Customer))]
        public async Task<IHttpActionResult> GetCustomer(string key)
        {
            Customer customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == key || c.Mobile == key);
            if (customer == null)
            {
                return Ok(new { custId = string.Empty });
            }
            var cust = new Models.Customer
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                Id = customer.Id,
                LastName = customer.LastName,
                Mobile = customer.Mobile
            };
            return Ok(cust);
        }

        // PUT: api/Customer/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customer
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbCust = db.Customers.SingleOrDefault(cust => cust.Email == customer.Email && cust.Mobile == customer.Mobile);
            if (dbCust == null)
            {
                customer.Id = Guid.NewGuid();
                db.Customers.Add(customer);
            }
            else
            {
                customer.Id = dbCust.Id;
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { custId = customer.Id });
        }

        // DELETE: api/Customer/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(Guid id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(Guid id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}