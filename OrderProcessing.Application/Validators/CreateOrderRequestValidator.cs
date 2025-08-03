using FluentValidation;
using OrderProcessing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator() 
        {
            // Validate Items
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.")
                .Must(items => items.All(item => item.Quantity > 0))
                .WithMessage("All items must have a quantity greater than zero.");

            // Validate Invoice Address, Email, and Credit Card
            RuleFor(x => x.InvoiceAddress)
                .NotNull().WithMessage("Invoice address is required.")
                .MinimumLength(10).WithMessage("Invoice address must be at least 10 characters long.")
                .MaximumLength(200).WithMessage("Invoice address must not exceed 200 characters.");

            RuleFor(x => x.InvoiceEmailAddress)
                .NotNull().WithMessage("Invoice email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.InvoiceCreditCard)
                .NotNull().WithMessage("Invoice credit card is required.")
                .CreditCard().WithMessage("Invalid credit card format.")
                .Must(card => card.Length == 16).WithMessage("Credit card number must be 16 digits long.");

        }

    }
}
