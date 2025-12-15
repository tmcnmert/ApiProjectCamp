using ApiProjectCamp.WebAPI.Dtos.FeatureDtos;
using ApiProjectCamp.WebAPI.Dtos.ProductDtos;
using ApiProjectCamp.WebAPI.Entities;
using AutoMapper;

namespace ApiProjectCamp.WebAPI.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<Feature,ResultFeatureDto>().ReverseMap();
            CreateMap<Feature,CreateFeatureDto>().ReverseMap();
            CreateMap<Feature,UpdateFeaturedTO>().ReverseMap();
            CreateMap<Feature, GetByIdFeatureDto>().ReverseMap();


            CreateMap<Message, ResultFeatureDto>().ReverseMap();
            CreateMap<Message, CreateFeatureDto>().ReverseMap();
            CreateMap<Message, UpdateFeaturedTO>().ReverseMap();
            CreateMap<Message, GetByIdFeatureDto>().ReverseMap();
            //AutoMapper'deki ReverseMap() metodu, iki yönlü dönüşüm sağlar. Yani, bir nesneyi diğerine dönüştürdüğünüz gibi, tersini de otomatik olarak yapmanıza olanak tanır.
            

            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductWithCategoryDto>().ForMember(x=>x.CategoryName,y=>y.MapFrom(z=>z.Category.CategoryName)).ReverseMap();
            //CategoryName'i Category tablosundan alır
        }
    }
}
