using System.Collections.Generic;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Repositories
{
    public class CustomerRepository
    {
        private string _connStr;

        public CustomerRepository(string connStr)
        {
            _connStr = connStr;
        }

        public CustomerResult GetCustomers(PagingInfo pagingInfo)
        {
            //TODO - make call to DB to retrieve customers using pagingInfo



            // dummy data for now
            var customers = new List<Customer>
            {
                new Customer
                {
                    CustomerId = 1,
                    FirstName = "John",
                    LastName = "Smith"  
                },
                new Customer
                {
                    CustomerId = 2,
                    FirstName = "Peter",
                    LastName = "Brown"
                },
            };




            return new CustomerResult
            {
                PagingInfo = pagingInfo,
                Customers = customers
            };
        }
    }
}
