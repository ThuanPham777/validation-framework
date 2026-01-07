using ValidationFramework.Fluent;
using ValidationFramework.Extensions;

namespace ValidationFramework.Demo.Validators
{
    public class ProductValidator : AbstractValidator<Models.Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
            .Required()
                .Length(3, 100)
                 .WithMessage("Product name must be 3-100 characters");

            RuleFor(p => p.SKU)
      .Required()
        .Regex(@"^[A-Z]{3}\d{3}$")
                .WithMessage("SKU must be 3 uppercase letters followed by 3 digits (e.g., ABC123)");

            RuleFor(p => p.Price)
                .GreaterThan(0m)
      .WithMessage("Price must be greater than 0");

            RuleFor(p => p.Stock)
          .Range(0, 10000)
                       .WithMessage("Stock must be between 0 and 10000");
        }
    }
}
