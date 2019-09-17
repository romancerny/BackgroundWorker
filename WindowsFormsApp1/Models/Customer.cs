using System.Collections.Generic;

namespace WindowsFormsApp1.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomerResult
    {
        public List<Customer> Customers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
