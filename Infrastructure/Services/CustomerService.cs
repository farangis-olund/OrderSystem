using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(CustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<CustomerEntity> AddCustomerAsync(CustomerEntity customer)
    {
        try
        {
            var existingCustomer = await _customerRepository.GetOneAsync(c => c.Email == customer.Email);

            if (existingCustomer != null)
            {
                return null!;
            }
            var newCustomer = new CustomerEntity { 
                Id = Guid.NewGuid(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber =customer.PhoneNumber};
            
            return await _customerRepository.AddAsync(newCustomer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding customer: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<CustomerEntity> GetCustomerAsync(string email)
    {
        try
        {
            return await _customerRepository.GetOneAsync(c => c.Email == email);
           
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding product image: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        try
        {
            var customerEntities = await _customerRepository.GetAllAsync(); 

            return customerEntities.Select(entity => (Customer)entity).ToList();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in getting all customers: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer)
    {
        try
        {
            var existingCustomer = await _customerRepository.GetOneAsync(c => c.Email == customer.Email);
                       
            if (existingCustomer !=null)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                return await _customerRepository.UpdateAsync(c => c.Email == customer.Email, existingCustomer);
            }
            else
                return null!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding customer order: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<bool> DeleteCustomerAsync(string email)
    {
        try
        {
            var existingCustomer = await _customerRepository.GetOneAsync(x => x.Email == email);

            if (existingCustomer != null)
            {
                await _customerRepository.RemoveAsync(c => c.Email== email);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in deleting customer: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

}
