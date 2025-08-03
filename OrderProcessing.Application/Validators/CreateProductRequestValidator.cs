using FluentValidation;
using OrderProcessing.Application.DTOs;

namespace OrderProcessing.Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator() 
        {           
            // Validate Product Name, Price, and Stock
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required.")
                .MinimumLength(10).WithMessage("Name must be at least 10 characters long.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required.")
                .Must(c => c > 0);


            RuleFor(x => x.Stock)
                .NotNull().WithMessage("Stock/Quantity is required.")
                .Must(c => c > 0);
        }
    }
}
