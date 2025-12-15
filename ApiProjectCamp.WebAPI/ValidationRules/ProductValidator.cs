using ApiProjectCamp.WebAPI.Entities;
using FluentValidation;

namespace ApiProjectCamp.WebAPI.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x=>x.ProductName).NotEmpty().WithMessage("Ürün Adı Boş Geçilemez!");
            RuleFor(x=>x.ProductName).MinimumLength(2).WithMessage("Ürün Adı En Az 2 Karakter Olmalıdır!");
            RuleFor(x=>x.ProductName).MaximumLength(50).WithMessage("Ürün Adı En Fazla 50 Karakter Olabilir!");

            RuleFor(x=>x.Price).NotEmpty().WithMessage("Ürün Fiyatı Boş Geçilemez").GreaterThan(0).WithMessage("Ürün Fiyatı 0'dan Az olamaz");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Ürün Açıklaması Boş Geçilemez!");

        }
    }
}
