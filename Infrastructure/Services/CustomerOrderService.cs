using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class CustomerOrderService
    {
        private readonly CustomerOrderRepository _customerOrderRepository;
        private readonly CustomerService _customerService;
        private readonly ILogger<CustomerOrderService> _logger;
        
        public CustomerOrderService(CustomerOrderRepository customerOrderRepository,
                                    CustomerService customerService,
                                    ILogger<CustomerOrderService> logger)
        {
            _customerOrderRepository = customerOrderRepository;
            _customerService = customerService;
            _logger = logger;
        }

        public async Task<CustomerOrderEntity> AddCustomerOrderAsync(CustomerOrder customerOrder)
        {
            try
            {
                var existingCustomer = await _customerService.GetCustomerAsync(customerOrder.CustomerEmail) 
                    ?? await _customerService.AddCustomerAsync(
                        new CustomerEntity { LastName = customerOrder.CustomerLastName, 
                                       FirstName = customerOrder.CustomerFirstName,
                                       Email = customerOrder.CustomerEmail,
                                       PhoneNumber =customerOrder.CustomerPhoneNumber});

                return await _customerOrderRepository.AddAsync(new CustomerOrderEntity
                {
                    TotalAmount = customerOrder.TotalAmount,
                    Date = customerOrder.Date,
                    CustomerId = existingCustomer.Id
                });
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CustomerOrderEntity> GetCustomerOrderAsync(CustomerOrder customerOrder)
        {
            try
            {
                return await _customerOrderRepository.GetOneAsync(co => co.Id == customerOrder.CustomerOrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CustomerOrderEntity> GetCustomerOrderByIdAsync(int id)
        {
            try
            {
                return await _customerOrderRepository.GetOneAsync(co => co.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<IEnumerable<CustomerOrderEntity>> GetAllCustomerOrdersAsync()
        {

            try
            {
                return await _customerOrderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting Customer Orders: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CustomerOrderEntity> UpdateCustomerOrderAsync(CustomerOrderEntity customerOrder)
        {
            try
            {  
                return await _customerOrderRepository.UpdateAsync(co => co.Id == customerOrder.Id, customerOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteCustomerOrderAsync(CustomerOrder customerOrder)
        {
            try
            {
              
                var existingCustomerOrder = await _customerOrderRepository.ExistsAsync(x => x.Equals(customerOrder));

                if (existingCustomerOrder)
                {
                    await _customerOrderRepository.RemoveAsync(customerOrder);
                }
                
                return existingCustomerOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCustomerOrderByIdAsync(int id)
        {
            try
            {

                var existingCustomerOrder = await _customerOrderRepository.GetOneAsync(x => x.Id == id);

                if (existingCustomerOrder != null)
                {
                    await _customerOrderRepository.RemoveAsync(existingCustomerOrder);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
