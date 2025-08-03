using FluentValidation;
using OrderProcessing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }

    }
}
