using FluentValidation;
using ServiceLayer.DTW.Domain.Models;

namespace ServiceLayer.DTW.Application.Validation;

public class BusinessPartnerValidator : AbstractValidator<BusinessPartner>
{
    public BusinessPartnerValidator()
    {
        RuleFor(bp => bp.CardCode)
            .NotEmpty().WithMessage("CardCode is required.")
            .MaximumLength(15).WithMessage("CardCode cannot exceed 15 characters.");

        RuleFor(bp => bp.CardName)
            .NotEmpty().WithMessage("CardName is required.")
            .MaximumLength(100).WithMessage("CardName cannot exceed 100 characters.");

        RuleFor(bp => bp.EmailAddress)
            .EmailAddress().When(bp => !string.IsNullOrWhiteSpace(bp.EmailAddress))
            .WithMessage("EmailAddress is not valid.");

        RuleForEach(bp => bp.Addresses).ChildRules(addr =>
        {
            addr.RuleFor(a => a.AddressType)
                .Must(t => t is "bo_BillTo" or "bo_ShipTo")
                .WithMessage("AddressType must be 'bo_BillTo' or 'bo_ShipTo'.");
        });
    }
}