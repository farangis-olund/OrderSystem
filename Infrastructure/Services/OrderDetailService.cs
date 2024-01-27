﻿using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly CustomerOrderService _customerOrderService;
        private readonly ProductVariantService _productVariantService;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(OrderDetailRepository orderDetailRepository, 
                                  CustomerOrderService customerOrderService,
                                  ProductVariantService productVariantService,
                                  ILogger<OrderDetailService> logger)
        {
            _orderDetailRepository = orderDetailRepository;
            _customerOrderService = customerOrderService;
            _productVariantService = productVariantService;
            _logger = logger;
        }

        public async Task<OrderDetailEntity> AddOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            try
            {
                var existingCustomerOrder = await _customerOrderService.AddCustomerOrderAsync(orderDetail.CustomerOrder);

                var existingProductVariant = await _productVariantService.AddProductVariantAsync(orderDetail.ProductVariant);
                
                if(existingCustomerOrder != null &&  existingProductVariant != null)
                {
                    var orderDetailExists = await _orderDetailRepository.ExistsAsync(
                            od => od.CustomerOrderId == existingCustomerOrder.Id &&
                            od.ProductVariantId == existingProductVariant.Id);

                    if (!orderDetailExists)
                    {
                        var newOrderDetail = new OrderDetailEntity
                        {
                            CustomerOrderId = existingCustomerOrder.Id,
                            ProductVariantId = existingProductVariant.Id,
                            Quantity = orderDetail.Quantity
                        };

                        return await _orderDetailRepository.AddAsync(newOrderDetail);
                    }
                }  
                
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<OrderDetailEntity> GetOrderDetailAsync(OrderDetailEntity orderDetail)
        {

            try
            {
                return await _orderDetailRepository.GetOneAsync(x => x.OrderDetailId == orderDetail.OrderDetailId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<IEnumerable<OrderDetailEntity>> GetAllOrderDetailsAsync()
        {

            try
            {
                return await _orderDetailRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }


        public async Task<OrderDetailEntity> UpdateOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            try
            {
                if (await _orderDetailRepository.ExistsAsync(x => x.OrderDetailId == orderDetail.OrderDetailId))
                {
                    return await _orderDetailRepository.UpdateAsync(x => x.OrderDetailId == orderDetail.OrderDetailId, orderDetail);
                }

                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }


        public async Task<bool> DeleteOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                if (await _orderDetailRepository.ExistsAsync(x => x.Equals(orderDetail)))
                {
                    await _orderDetailRepository.RemoveAsync(orderDetail);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
