using ApiProjectCamp.WebAPI.Dtos.AboutDtos;
using ApiProjectCamp.WebAPI.Dtos.CategoryDtos;
using ApiProjectCamp.WebAPI.Dtos.FeatureDtos;
using ApiProjectCamp.WebAPI.Dtos.ImageDtos;
using ApiProjectCamp.WebAPI.Dtos.MessageDtos;
using ApiProjectCamp.WebAPI.Dtos.NotificationDtos;
using ApiProjectCamp.WebAPI.Dtos.ProductDtos;
using ApiProjectCamp.WebAPI.Dtos.ReservationDtos;
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


            CreateMap<Message, ResultMessageDto>().ReverseMap();
            CreateMap<Message, CreateMessageDto>().ReverseMap();
            CreateMap<Message, UpdateMessageDto>().ReverseMap();
            CreateMap<Message, GetByIdMessageDto>().ReverseMap();

            //AutoMapper'deki ReverseMap() metodu, iki yönlü dönüşüm sağlar. Yani, bir nesneyi diğerine dönüştürdüğünüz gibi, tersini de otomatik olarak yapmanıza olanak tanır.



            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductWithCategoryDto>().ForMember(x=>x.CategoryName,y=>y.MapFrom(z=>z.Category.CategoryName)).ReverseMap();
            //CategoryName'i Category tablosundan alır

            CreateMap<Notification,ResultNotificationDto>().ReverseMap();
            CreateMap<Notification,CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
            CreateMap<Notification, GetNotificationByIdDto>().ReverseMap();


            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();

            CreateMap<About,ResultAboutDto>().ReverseMap();
            CreateMap<About,CreateAboutDto>().ReverseMap();
            CreateMap<About,UpdateAboutDto>().ReverseMap();
            CreateMap<About,GetAboutByIdDto>().ReverseMap();


            CreateMap<Reservation,ResultReservationDto>().ReverseMap();
            CreateMap<Reservation,CreateReservationDto>().ReverseMap();
            CreateMap<Reservation,GetReservationByIdDto>().ReverseMap();
            CreateMap<Reservation,UpdateReservationDto>().ReverseMap();


            CreateMap<Image,ResultImageDto>().ReverseMap();
            CreateMap<Image,CreateImageDto>().ReverseMap();
            CreateMap<Image,UpdateImageDto>().ReverseMap();
            CreateMap<Image,GetImageByIdDto>().ReverseMap();


        }
    }
}
