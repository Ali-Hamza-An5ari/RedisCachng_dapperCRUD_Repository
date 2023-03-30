using Dapper;
using dapperCRUD.Models;
using dapperCRUD.Services.CustomerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;

namespace dapperCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository; 
        private readonly IDistributedCache _distributedCache;
        string RedisCacheKey = "Master1";
        public CustomerController(ICustomerRepository repository, IDistributedCache distributedCache)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }
     
        [HttpGet("/GetAll")]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            ; //await _repository.GetAllCustomers();

            IEnumerable<Customer> customers = new List<Customer>();

            try
            {
                string SerializeList = string.Empty;
                var EncodedList = await _distributedCache.GetAsync(RedisCacheKey);
                if (EncodedList != null)
                {
                    await _distributedCache.RemoveAsync(RedisCacheKey);
                    //customers = new List<Customer>();
                    SerializeList = Encoding.UTF8.GetString(EncodedList);
                    customers = JsonConvert.DeserializeObject<List<Customer>>(SerializeList);
                }
                else
                {
                    customers = await _repository.GetAllCustomers();
                    
                        SerializeList = JsonConvert.SerializeObject(customers);
                        EncodedList = Encoding.UTF8.GetBytes(SerializeList);
                        var Option = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20)) // After 20 min Entry will be Inactive
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6)); // Expired in 6 hour
                        await _distributedCache.SetAsync(RedisCacheKey, EncodedList, Option);
                    
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                //response.IsSuccess = false;
                //response.Message = ex.Message;
                return BadRequest(ex.Message);
            }
            
            
        }


        [HttpGet("/Get/{customerId}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid customerId)
        {
            
            var searchedCustomer = await _repository.GetCustomerById(customerId);

            if (searchedCustomer == null)
            {
                return NotFound();
            }

            return Ok(searchedCustomer);
        }


        [HttpPost("Create")]
        public async Task<ActionResult<List<Customer>>> CreateCustomer(Customer customer)
        {
            await _repository.CreateCustomer(customer);
            return Ok(await _repository.GetCustomerById(customer.Id));
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<List<Customer>>> UpdateCustomer(Customer customer)
        {
            var searchedCustomer = await _repository.GetCustomerById(customer.Id);

            if (searchedCustomer == null)
            {
                return NotFound();
            }
            await _repository.UpdateCustomer(customer);
            return Ok(await _repository.GetAllCustomers());
        }


        [HttpDelete("/Delete/{customerId}")]
        public async Task<ActionResult<List<Customer>>> DeleteCustomer(Guid customerId)
        {

            // Check if the customer exists
             var searchedCustomer = await _repository.GetCustomerById(customerId);

            if (searchedCustomer == null)
            {
                return NotFound();
            }
            await _repository.DeleteCustomer(customerId);

            return Ok(await _repository.GetAllCustomers());
        }

        [HttpGet("/GetIdName")]
        public async Task<ActionResult<List<string>>> GetIdNameCustomers()
        {

            IEnumerable<Customer> customers = await _repository.GetAllCustomers();
            List<string> names = new List<string>();
            foreach (Customer customer in customers)
            {
                names.Add(customer.Name);
            }
            return Ok(names);
        }

        [HttpGet("/GetIdName{customerId}")]
        public async Task<ActionResult<string>> GetNameCustomersId(Guid customerId)
        {

           Customer customer = await _repository.GetCustomerById(customerId);
           
            return Ok(customer.Name);
        }
    }
}
