using Grpc.Core;
using DIscount.Grpc;
using DIscount.Grpc.Protos;
using DIscount.Grpc.Repositories;
using AutoMapper;
using DIscount.Grpc.Entities;

namespace DIscount.Grpc.Services
{
    public class DIscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DIscountService> _logger;
        public DIscountService(ILogger<DIscountService> logger, IDiscountRepository discountRepository, IMapper mapper)
        {
            _logger = logger;
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name {request.ProductName} not found"));
            }

            _logger.LogInformation($"Coupon retrieved for {request.ProductName} with amount {coupon.Amount}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.CreateDiscount(coupon);
            _logger.LogInformation($"Coupon created successfully with name {request.Coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.UpdateDiscount(coupon);
            _logger.LogInformation($"Coupon updated successfully with name {request.Coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}