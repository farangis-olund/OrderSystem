﻿using Infrastructure.Dtos;
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

        public async Task<CustomerOrderEntity?> AddCustomerOrder(CustomerOrder customerOrder, Customer customer)
        {
            try
            {
                var existingCustomer = await _customerService.GetCustomer(customer.Email) 
                    ?? await _customerService.AddCustomer(customer);

                if (existingCustomer == null)
                {
                    _logger.LogError("Failed to add or retrieve the customer.");
                    return null;
                }

                var newCustomerOrder = new CustomerOrderEntity
                {
                    TotalAmount = customerOrder.TotalAmount,
                    Date = customerOrder.Date,
                    Customer = existingCustomer
                };
                    
                return await _customerOrderRepository.AddAsync(newCustomerOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<CustomerOrderEntity> GetCustomerOrderById(int id)
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

        public async Task<CustomerOrderEntity?> UpdateCustomerOrder(int id, CustomerOrder customerOrder)
        {
            try
            {
                var existingCustomerOrder = await _customerOrderRepository.Exist(co => co.Id == id);
                if (existingCustomerOrder)
                {
                    return await _customerOrderRepository.UpdateAsync(customerOrder);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteCustomerOrder(CustomerOrder customerOrder)
        {
            try
            {
              
                var existingCustomerOrder = await _customerOrderRepository.Exist(x => x.Equals(customerOrder));

                if (existingCustomerOrder)
                {
                    await _customerOrderRepository.RemoveAsync(customerOrder);
                    return true;
                }
                else
                    return false;
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