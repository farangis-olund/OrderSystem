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
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(OrderDetailRepository orderDetailRepository, 
                                  ILogger<OrderDetailService> logger)
        {
            _orderDetailRepository = orderDetailRepository;
            _logger = logger;
        }

        public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                var orderDetailExists = await _orderDetailRepository.GetOneAsync(
                        od => od.CustomerOrderId == orderDetail.CustomerOrderId &&
                        od.ProductVariantId == orderDetail.ProductVariantId);

                if (orderDetailExists != null)
                {
                    return null!;
                }
                var newOrderDetailEntity = new OrderDetailEntity
                {
                    CustomerOrderId = orderDetail.CustomerOrderId,
                    ProductVariantId = orderDetail.ProductVariantId,
                    Quantity = orderDetail.Quantity

                };

                var orderDetailEntity = await _orderDetailRepository.AddAsync(newOrderDetailEntity);
                return new OrderDetail
                {
                    OrderDetailId = orderDetailEntity.OrderDetailId,
                    CustomerOrderId = orderDetailEntity.CustomerOrderId,
                    ProductVariantId = orderDetailEntity.ProductVariantId,
                    Quantity = orderDetailEntity.Quantity
                };
                
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

        public async Task<OrderDetail> GetOrderDetailByCustomerOrderIdAsync(int customerOrderId)
        {

            try
            {
                return await _orderDetailRepository.GetOneAsync(x => x.CustomerOrderId == customerOrderId);
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
                if (await _orderDetailRepository.GetOneAsync(x => x.OrderDetailId == orderDetail.OrderDetailId) != null)
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
                var deleteOrderDetailEntity = await _orderDetailRepository.GetOneAsync(x => x.OrderDetailId == orderDetail.OrderDetailId);
                if (deleteOrderDetailEntity != null)
                {
                    await _orderDetailRepository.RemoveAsync(deleteOrderDetailEntity);
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

        public async Task<bool> DeleteOrderDetailByOrderIdAsync(int orderId)
        {
            try
            {
                var deleteOrderDetail = await _orderDetailRepository.GetOneAsync(x => x.CustomerOrderId == orderId);
                if (deleteOrderDetail != null)
                {
                    await _orderDetailRepository.RemoveAsync(deleteOrderDetail);
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
