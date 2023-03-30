using Dapper;
using dapperCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace dapperCRUD.Services.CustomerService
{

    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Customer>("EXEC getAllCustomers");
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>("EXEC getCustomerById @Id=@Id",
                new { Id = customerId });
        }

        public async Task<int> CreateCustomer(Customer customer)
        {
            using var connection = GetConnection();
            //customer.Id = Guid.NewGuid();
            return await connection.ExecuteAsync("EXEC insertCustomer @Name=@Name, @Email=@Email, @DateOfBirth=@DateOfBirth",
                customer);
        }

        public async Task<int> UpdateCustomer(Customer customer)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync("EXEC updateCustomer  @Id=@Id, @Name=@Name, @Email=@Email, @DateOfBirth=@DateOfBirth",
                customer);
        }


        public async Task<int> DeleteCustomer(Guid customerId)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync("EXEC deleteCustomer  @Id=@Id", new { Id = customerId });
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
