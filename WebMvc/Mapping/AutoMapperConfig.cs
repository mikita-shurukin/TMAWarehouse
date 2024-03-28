using AutoMapper;
using WebMvc.DAL.Models;
using WebMvc.Models.Requests;
using WebMvc.Models.Requests.ItemGroup;
using WebMvc.Models.Requests.Orders;

namespace WebMvc.Mapping
{
    public class AutoMapperConfig: Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CreateWarehouseRequest, Warehouse>();
            CreateMap<UpdateWarehouseRequest, Warehouse>();

            CreateMap<CreateItemRequest, Item>();
            CreateMap<UpdateItemRequest, Item>();


            CreateMap<CreateOrderRequest, Order>();

            CreateMap<CreateItemGroupRequest, ItemGroup>();
            CreateMap<UpdateItemGroupRequest, ItemGroup>();

            CreateMap<string, ItemGroup>().ConstructUsing(name => new ItemGroup { Name = name });

        }
    }
}
