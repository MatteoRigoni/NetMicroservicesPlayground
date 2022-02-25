using AutoMapper;
using DIscount.Grpc.Entities;
using DIscount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class DiscountProfile: Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
