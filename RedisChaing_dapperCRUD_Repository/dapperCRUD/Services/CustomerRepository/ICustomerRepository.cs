using dapperCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace dapperCRUD.Services.CustomerService
{
    public interface ICustomerRepository 
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(Guid customerId);
        Task<int> CreateCustomer(Customer customer);
        Task<int> UpdateCustomer(Customer customer);
        Task<int> DeleteCustomer(Guid customerId);


    }
}
