using AutoFixture;
using FluentAssertions;
using InventoryMg.API.Controllers;
using InventoryMg.BLL.DTOs.Request;
using InventoryMg.BLL.DTOs.Response;
using InventoryMg.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryMg.API.Test.Controllers;

public class SaleControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<ISalesServices> _serviceMock;
    private readonly SaleController _saleController;

    public SaleControllerTest()
    {
        _fixture = new Fixture();
        _serviceMock = _fixture.Freeze<Mock<ISalesServices>>();
        _saleController = new SaleController(_serviceMock.Object);
    }


    [Fact]
    public async Task GetUserSales_ShouldReturnOkResponse_WhenDataFound()
    {
        //Arrange
        var salesMock = _fixture.Create<IEnumerable<SalesResponseDto>>();
        _serviceMock.Setup(x => x.GetUserSales()).ReturnsAsync(salesMock);
        //Act
        var result = await _saleController.GetUserSales().ConfigureAwait(false);

        //Assert
        /*Assert.NotNull(result);*/
        result.Should().NotBeNull(); //fluent
        /* result.Should().BeAssignableTo<IEnumerable<SalesResponseDto>>();*/
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(salesMock.GetType());
        _serviceMock.Verify(x => x.GetUserSales(), Times.Once());

    }

    [Fact]
    public async Task GetUserSales_ShouldReturnNotFound_WhenDataIsNotFound()
    {
        //Arrange
        IEnumerable<SalesResponseDto> response = null;
        _serviceMock.Setup(x => x.GetUserSales()).ReturnsAsync(response);
        //Act
        var result = await _saleController.GetUserSales().ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<NotFoundResult>();
        //_serviceMock.Verify(x => x.GetUserSales(), Times.Never());
    }

    [Fact]
    public async Task GetSaleById_ShouldReturnOkResponse_WhenValidInput()
    {
        //Arrange
        var salesMock = _fixture.Create<SalesResponseDto>();
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.GetSaleById(id)).ReturnsAsync(salesMock);

        //Act
        var result = await _saleController.GetSaleById(id.ToString()).ConfigureAwait(false);

        //Assert
        /*Assert.NotNull(result);*/
        result.Should().NotBeNull(); //fluent
        /* result.Should().BeAssignableTo<ActionResult<SalesResponseDto>>();*/
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(salesMock.GetType());
        _serviceMock.Verify(x => x.GetSaleById(id), Times.AtLeastOnce());

    }
    [Fact]
    public async Task GetSaleById_ShouldReturnNotFound_WhenNoDataFound()
    {
        //Arrange
        SalesResponseDto salesMock = null;
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.GetSaleById(id)).ReturnsAsync(salesMock);

        //Act
        var result = await _saleController.GetSaleById(id.ToString()).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<NotFoundResult>();
        _serviceMock.Verify(x => x.GetSaleById(id), Times.AtLeastOnce());

    }

    [Fact]
    public async Task AddSale_ShouldReturnOkResponse_WhenValidRequest()
    {
        //Arrange
        var request = _fixture.Create<SalesRequestDto>();
        var response = _fixture.Create<SalesResponseDto>();
        _serviceMock.Setup(x => x.AddSale(request)).ReturnsAsync(response);

        //Act
        var result = await _saleController.AddSale(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        /* result.Should().BeAssignableTo<ActionResult<SalesResponseDto>>();*/
        result.Should().BeAssignableTo<ObjectResult>();
        result.As<ObjectResult>().Value.Should().NotBeNull().And.BeOfType(response.GetType());
        _serviceMock.Verify(x => x.AddSale(request), Times.AtLeastOnce());

    }
    [Fact]
    public async Task AddSale_ShouldReturnBadRequest_WhenInValidRequest()
    {
        //Arrange
        var request = _fixture.Create<SalesRequestDto>();
        _saleController.ModelState.AddModelError("Name", "The Name Feild is required");
        var response = _fixture.Create<SalesResponseDto>();
        _serviceMock.Setup(x => x.AddSale(request)).ReturnsAsync(response);

        //Act
        var result = await _saleController.AddSale(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestResult>();
        _serviceMock.Verify(x => x.AddSale(request), Times.Never());

    }

    [Fact]
    public async Task DeleteSale_ShouldReturnNoContent_WhenDeletedARecord()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.DeleteSale(id)).ReturnsAsync(true);

        //Act

        var result = await _saleController.Delete(id.ToString()).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<NoContentResult>();

    }

    [Fact]
    public async Task DeleteSale_ShouldReturnBadRequest_WhenDeletedARecordFails()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.DeleteSale(id)).ReturnsAsync(false);

        //Act

        var result = await _saleController.Delete(id.ToString()).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
        _serviceMock.Verify(x => x.DeleteSale(id), Times.AtLeastOnce());
    }

    [Fact]
    public async Task UpdateSale_ShouldReturnOkResponse_WhenRequestIsValid()
    {
        //Arrange 
        var request = _fixture.Create<SalesResponseDto>();

        _serviceMock.Setup(x => x.EditSale(request)).ReturnsAsync(request);

        //Act
        var result = await _saleController.UpdateSale(request).ConfigureAwait(false);
        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OkObjectResult>();
        /*_serviceMock.Verify(x => x.EditSale(request),Times.Never()); */
    }

    [Fact]
    public async Task UpdateSale_ShouldReturnBasRequest_WhenRequestIsInvalid()
    {
        //Arrange
        var request = _fixture.Create<SalesResponseDto>();
        _saleController.ModelState.AddModelError("Name", "Name is required");
        _serviceMock.Setup(x => x.EditSale(request)).ReturnsAsync(request);

        //Act
        var result = await _saleController.UpdateSale(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestResult>();
        _serviceMock.Verify(x => x.EditSale(request), Times.Never());

    }
}


