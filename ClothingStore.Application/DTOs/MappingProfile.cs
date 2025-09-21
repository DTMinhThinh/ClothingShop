using AutoMapper;
using ClothingStore.Application.DTOs.Customer;
using ClothingStore.Application.DTOs.Product;
using ClothingStore.Domain.Entities;

namespace ClothingStore.Application
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Ánh xạ từ Customer sang CustomerDTO
			CreateMap<Customer, CreateCustomerDTO>();
			CreateMap<CreateCustomerDTO, Customer>();
			CreateMap<Customer, UpdateProfileCustomerDTO>();
			CreateMap<UpdateProfileCustomerDTO, Customer>();
			// Nếu cần ánh xạ ngược từ DTO về Entity, bạn có thể thêm ánh xạ này:
			// Map từ Product sang CreateProductDTO (bỏ qua ID)
			CreateMap<Product, CreateProductDTO>()
				.ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
				.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

			// Map từ UpdateProductDTO sang Product entity
			CreateMap<UpdateProductDTO, Product>()
		   .ForMember(dest => dest.ProductId, opt => opt.Ignore()) // Không map ID
		   .ForMember(dest => dest.Quantity, opt => opt.Ignore())  // Không map Quantity
		   .ForMember(dest => dest.CreateDate, opt => opt.Ignore()); // Không map CreateDate
		}
	}
}
