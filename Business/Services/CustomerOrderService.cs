using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business.Services
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

        public async Task<CustomerOrderEntity?> AddCustomerOrder(CustomerOrder customerOrder)
        {
            try
            {
                var existingCustomer = await _customerService.GetCustomer(customerOrder.Customer) 
                    ?? await _customerService.AddCustomer(customerOrder.Customer);

                if (existingCustomer == null)
                {
                    _logger.LogError("Failed to add or retrieve the customer.");
                    return null;
                }

                var customerOrderExists = await _customerOrderRepository.Exist(co => co.Id == customerOrder.Id && co.CustomerId == existingCustomer.Id);

                if (!customerOrderExists)
                {
                    var newCustomerOrder = new CustomerOrderEntity
                    {
                        TotalAmount = customerOrder.TotalAmount,
                        Date = customerOrder.Date,
                        Customer = existingCustomer
                    };
                    
                    return await _customerOrderRepository.AddAsync(newCustomerOrder);
                }

                _logger.LogError($"Info: Customer order already exists");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<CustomerOrderEntity?> GetCustomerOrder(CustomerOrder customerOrder)
        {
            try
            {
                return await _customerOrderRepository.GetOneAsync(co => co.Id == customerOrder.Id && 
                                                co.CustomerId == customerOrder.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<CustomerOrderEntity?> GetCustomerOrderById(int id)
        {
            try
            {
                return await _customerOrderRepository.GetOneAsync(co => co.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
