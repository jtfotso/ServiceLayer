using FluentAssertions;
using ServiceLayer.DTW.Application.Validation;
using ServiceLayer.DTW.Domain.Models;

namespace ServiceLayer.DTW.Application.Tests;

public class BusinessPartnerValidatorTests
{
    private readonly BusinessPartnerValidator _sut = new();

    [Fact]
    public async Task Should_Fail_When_CardCode_Is_Empty()
    {
        var bp = new BusinessPartner { CardCode = "", CardName = "Test" };
        var result = await _sut.ValidateAsync(bp);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CardCode");
    }

    [Fact]
    public async Task Should_Fail_When_Email_Is_Invalid()
    {
        var bp = new BusinessPartner { CardCode = "C001", CardName = "Test", EmailAddress = "not-an-email" };
        var result = await _sut.ValidateAsync(bp);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "EmailAddress");
    }

    [Fact]
    public async Task Should_Pass_For_Valid_Business_Partner()
    {
        var bp = new BusinessPartner { CardCode = "C001", CardName = "Acme Corp", EmailAddress = "info@acme.com" };
        var result = await _sut.ValidateAsync(bp);
        result.IsValid.Should().BeTrue();
    }
}