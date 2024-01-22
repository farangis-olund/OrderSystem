using Infrastructure.Dtos;
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

        public async Task<OrderDetailEntity?> AddOrderDetail(OrderDetail orderDetail, int customerOrderId, int productVariantId)
        {
            try
            {
                var existingCustomerOrder = await _customerOrderService.GetCustomerOrderById(customerOrderId);

                var existingProductVariant = await _productVariantService.GetProductVariantById(productVariantId);
                
                if(existingCustomerOrder != null &&  existingProductVariant != null)
                {
                    var orderDetailExists = await _orderDetailRepository.Exist(
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
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding customer order: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<OrderDetailEntity> GetOrderDetail(OrderDetail orderDetail)
        {

            try
            {
                var existingOrderDetail = await _orderDetailRepository.GetOneAsync(x => x.Equals(orderDetail));

                if (existingOrderDetail != null)
                {
                    return existingOrderDetail;
                }
                else
                {
                    Debug.WriteLine("Order detail does not exist!");
                    return null!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<OrderDetailEntity> GetOrderDetail(int id)
        {

            try
            {
                var existingOrderDetail = await _orderDetailRepository.GetOneAsync(c => c.OrderDetailId == id);

                if (existingOrderDetail != null)
                {
                    return existingOrderDetail;
                }
                else
                {
                    Debug.WriteLine("Order detail does not exist!");
                    return null!;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in geting Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<OrderDetailEntity?> UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                var existingOrderDetail = await _orderDetailRepository.Exist(x => x.Equals(orderDetail));
                if (existingOrderDetail)
                {
                    return await _orderDetailRepository.UpdateAsync(orderDetail);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating Order detail: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {

                var existingOrderDetail = await _orderDetailRepository.Exist(x => x.Equals(orderDetail));

                if (existingOrderDetail)
                {
                    await _orderDetailRepository.RemoveAsync(orderDetail);
                    return true;
                }
                else
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
