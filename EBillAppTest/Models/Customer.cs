using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBillAppTest.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Guid Id { get; set; }
    }
}