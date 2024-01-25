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

    public async Task<CustomerEntity> AddCustomerAsync(Customer customer)
    {

        try
        {
            var existingCustomer = await _customerRepository.Exist(c => c.Email == customer.Email);

            if (!existingCustomer)
            {
                var newCustomer = new CustomerEntity
                {
                   FirstName = customer.FirstName,
                   LastName = customer.LastName,
                   Email = customer.Email,
                   PhoneNumber = customer.PhoneNumber
                };
                
                return await _customerRepository.AddAsync(newCustomer);
            }
            return null!;
           
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding product image: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<CustomerEntity> GetCustomerAsync(Customer customer)
    {

        try
        {
            var existingCustomer= await _customerRepository.GetOneAsync(c => c.Email == customer.Email);

            if (existingCustomer != null)
            {
                return existingCustomer;
            }
            else
            {
                Debug.WriteLine("Customer does not exist!");
                return null!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in geting customer: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<CustomerEntity> GetCustomerAsync(string email)
    {

        try
        {
            var existingCustomer = await _customerRepository.GetOneAsync(c => c.Email == email);

            if (existingCustomer != null)
            {
                return existingCustomer;
            }
            else
            {
                Debug.WriteLine("Customer does not exist!");
                return null!;
            }


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding product image: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<CustomerEntity> UpdateCustomerAsync(Customer customer)
    {
        try
        {
            var existingCustomerOrder = await _customerRepository.Exist(c => c.Email == customer.Email);
            if (existingCustomerOrder)
            {
                Func<CustomerEntity, object> keySelector = p => p.Id;
                return await _customerRepository.UpdateAsync(customer, keySelector);
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

    public async Task<bool> DeleteCustomerAsync(Customer customer)
    {
        try
        {

            var existingCustomerOrder = await _customerRepository.Exist(x => x.Equals(customer));

            if (existingCustomerOrder)
            {
                await _customerRepository.RemoveAsync(customer);
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
