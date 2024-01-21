using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business.Services
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

        public async Task<OrderDetailEntity?> AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                var existingCustomerOrder = await _customerOrderService.GetCustomerOrder(orderDetail.CustomerOrder)
                    ?? await _customerOrderService.AddCustomerOrder(orderDetail.CustomerOrder);

                var existingProductVariant = await _productVariantService.GetProductVariant(orderDetail.ProductVariant)
                   ?? await _productVariantService.AddProductVariant(orderDetail.ProductVariant);

                var orderDetailExists = await _orderDetailRepository.Exist(
                            od => od.CustomerOrderId == orderDetail.CustomerOrder.Id && 
                            od.ProductVariantId == orderDetail.ProductVariant.Id);

                if (!orderDetailExists)
                {
                    var newOrderDetail = new OrderDetailEntity
                    {
                        CustomerOrderId = orderDetail.CustomerOrder.Id, 
                        ProductVariantId = orderDetail.ProductVariant.Id,
                        Quantity = orderDetail.Quantity
                };

                    return await _orderDetailRepository.AddAsync(newOrderDetail);
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
    }
}
