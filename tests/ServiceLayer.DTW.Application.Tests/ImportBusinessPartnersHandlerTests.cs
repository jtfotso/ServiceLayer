using FluentValidation;
using FluentValidation.Results;
using Moq;
using ServiceLayer.DTW.Application.DTOs;
using ServiceLayer.DTW.Application.Interfaces;
using ServiceLayer.DTW.Application.UseCases.ImportBusinessPartners;
using ServiceLayer.DTW.Domain.Enums;
using ServiceLayer.DTW.Domain.Models;
using ServiceLayer.DTW.Infrastructure.Mapping;

namespace ServiceLayer.DTW.Application.Tests;

public class ImportBusinessPartnersHandlerTests
{
    [Fact]
    public async Task Should_Return_Success_For_Valid_Row()
    {
        var parserMock    = new Mock<IFileParser>();
        var slClientMock  = new Mock<IServiceLayerClient>();
        var validatorMock = new Mock<IValidator<BusinessPartner>>();
        var mapperMock    = new Mock<IBusinessPartnerMapper>();

        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<BusinessPartner>(), default))
            .ReturnsAsync(new ValidationResult());

        slClientMock
            .Setup(s => s.BusinessPartnerExistsAsync("C001", default))
            .ReturnsAsync(false);

        var parsedRows = new List<ParsedRowDto>
        {
            new() { RowNumber = 1, Fields = new(StringComparer.OrdinalIgnoreCase) {
                { "CardCode", "C001" }, { "CardName", "Acme" }, { "CardType", "C" }
            }}
        };

        parserMock
            .Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<string>(), default))
            .ReturnsAsync(parsedRows);

        var resolverMock = new Mock<IFileParserResolver>();
        resolverMock
            .Setup(r => r.Resolve(It.IsAny<string>()))
            .Returns(parserMock.Object);

        mapperMock
            .Setup(m => m.ToDomain(It.IsAny<ParsedRowDto>()))
            .Returns(new BusinessPartner
            {
                CardCode = "C001",
                CardName = "Acme",
                CardType = CardType.Customer
            });

        var handler = new ImportBusinessPartnersHandler(
            resolverMock.Object, slClientMock.Object, validatorMock.Object, mapperMock.Object);

        var command = new ImportBusinessPartnersCommand(
            FileStream:  new MemoryStream(),
            FileName:    "test.csv",
            Mode:        ImportMode.Upsert,
            StopOnError: false);

        var result = await handler.Handle(command, default);

        Assert.Equal(1, result.SuccessCount);
        Assert.Equal(0, result.ErrorCount);
    }

    [Fact]
    public void Should_Extract_Udf_Fields_From_Flat_Row()
    {
        // Verifies that U_* columns are captured by the mapper
        var row = new ParsedRowDto
        {
            RowNumber = 1,
            Fields    = new(StringComparer.OrdinalIgnoreCase)
            {
                { "CardCode",        "C001"      },
                { "CardName",        "Acme Corp" },
                { "CardType",        "C"         },
                { "U_TaxRegion",     "Northeast" },
                { "U_CustomerTier",  "Gold"      }
            }
        };

        var mapper = new BusinessPartnerMapper();
        var bp = mapper.ToDomain(row);

        Assert.Equal("Northeast", bp.UdfFields["U_TaxRegion"]);
        Assert.Equal("Gold",      bp.UdfFields["U_CustomerTier"]);
    }

    [Fact]
    public void Should_Map_BillTo_And_ShipTo_Addresses_From_Flat_Row()
    {
        var row = new ParsedRowDto
        {
            RowNumber = 1,
            Fields    = new(StringComparer.OrdinalIgnoreCase)
            {
                { "CardCode",        "C001"        },
                { "CardName",        "Acme Corp"   },
                { "CardType",        "C"           },
                { "BillTo_Street",   "123 Main St" },
                { "BillTo_City",     "New York"    },
                { "BillTo_Country",  "US"          },
                { "ShipTo_Street",   "456 Whse Ave"},
                { "ShipTo_City",     "Brooklyn"    },
                { "ShipTo_Country",  "US"          }
            }
        };

        var mapper = new BusinessPartnerMapper();
        var bp = mapper.ToDomain(row);

        Assert.Equal(2, bp.Addresses.Count);
        Assert.Contains(bp.Addresses, a => a.AddressType == "bo_BillTo" && a.City == "New York");
        Assert.Contains(bp.Addresses, a => a.AddressType == "bo_ShipTo" && a.City == "Brooklyn");
    }
}
