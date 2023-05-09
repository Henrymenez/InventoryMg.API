using AutoFixture;
using FluentAssertions;
using InventoryMg.API.Controllers;
using InventoryMg.BLL.DTOs.Request;
using InventoryMg.BLL.DTOs.Response;
using InventoryMg.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryMg.API.Test.Controllers;

public class ProductControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductController _prodController;

    public ProductControllerTest()
    {
        _fixture = new Fixture();
        _serviceMock = _fixture.Freeze<Mock<IProductService>>();
        _prodController = new ProductController(_serviceMock.Object);

    }


    [Fact]
    public async Task GetProducts_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange
        var prodMock = _fixture.Create<IEnumerable<ProductView>>();
        _serviceMock.Setup(x => x.GetAllUserProducts()).ReturnsAsync(prodMock);

        //Act 
        var result = await _prodController.GetProducts().ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(prodMock.GetType());

    }

    [Fact]
    public async Task GetProducts_ShouldReturnNotFound_WhenNoDataIsReturned()
    {
        IEnumerable<ProductView> prodMock = null;
        _serviceMock.Setup(x => x.GetAllUserProducts()).ReturnsAsync(prodMock);

        //Act 
        var result = await _prodController.GetProducts().ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<NotFoundResult>();

    }

    [Fact]
    public async Task Addproduct_ShouldReturnOkResponse_WhenValidRequest()
    {
        //Arrange
        var request = _fixture.Create<ProductViewRequest>();
        var response = _fixture.Create<ProductResult>();
        _serviceMock.Setup(x => x.AddProductAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _prodController.Addproduct(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ObjectResult>();
        result.As<ObjectResult>().Value.Should().NotBeNull().And.BeOfType(response.GetType());
        _serviceMock.Verify(x => x.AddProductAsync(request), Times.AtLeastOnce());

    }
    [Fact]
    public async Task Addproduct_ShouldReturnBadRequest_WhenInValidRequest()
    {
        //Arrange
        var request = _fixture.Create<ProductViewRequest>();
        _prodController.ModelState.AddModelError("Name", "The Name Feild is required");
        var response = _fixture.Create<ProductResult>();
        _serviceMock.Setup(x => x.AddProductAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _prodController.Addproduct(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestResult>();
        _serviceMock.Verify(x => x.AddProductAsync(request), Times.Never());

    }

    [Fact]
    public async Task Addproduct_ShouldReturnBadRequestObject_WhenInValidRequest()
    {
        //Arrange
        var request = _fixture.Create<ProductViewRequest>();

        ProductResult response = new()
        {
            Result = false
        };
        _serviceMock.Setup(x => x.AddProductAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _prodController.Addproduct(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
        _serviceMock.Verify(x => x.AddProductAsync(request), Times.AtLeastOnce());

    }

    [Fact]
    public async Task GetProductById_ShouldReturnOkResponse_WhenValidRequest()
    {
        //Arrange
        var prodMock = _fixture.Create<ProductView>();
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.GetProductById(id)).ReturnsAsync(prodMock);

        //Act
        var result = await _prodController.GetProductById(id.ToString()).ConfigureAwait(false);

        //Assert
        /*Assert.NotNull(result);*/
        result.Should().NotBeNull(); //fluent
        /* result.Should().BeAssignableTo<ActionResult<SalesResponseDto>>();*/
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(prodMock.GetType());
        _serviceMock.Verify(x => x.GetProductById(id), Times.AtLeastOnce());
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenNoDataFound()
    {
        //Arrange
        ProductView prodMock = null;
        var id = _fixture.Create<Guid>();
        _serviceMock.Setup(x => x.GetProductById(id)).ReturnsAsync(prodMock);

        //Act
        var result = await _prodController.GetProductById(id.ToString()).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<NotFoundResult>();
        _serviceMock.Verify(x => x.GetProductById(id), Times.AtLeastOnce());

    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnOkResponse_WhenValidRequest()
    {
        //Arrange
        var request = _fixture.Create<ProductView>();
        ProductResult response = new()
        {
            Result = true
        };

        _serviceMock.Setup(x => x.EditProductAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _prodController.UpdateProduct(request).ConfigureAwait(false);

        //Assert
        /*Assert.NotNull(result);*/
        result.Should().NotBeNull(); //fluent
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(response.GetType());
        _serviceMock.Verify(x => x.EditProductAsync(request), Times.AtLeastOnce());
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenRequestInvalid()
    {
        //Arrange
        var request = _fixture.Create<ProductView>();
        ProductResult response = new()
        {
            Result = false
        };
       
        _serviceMock.Setup(x => x.EditProductAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _prodController.UpdateProduct(request).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
        _serviceMock.Verify(x => x.EditProductAsync(request), Times.AtLeastOnce());

    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenValidRequest()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
       ProductResult response = new()
        {
            Result = true
        };


        _serviceMock.Setup(x => x.DeleteProductAsync(id)).ReturnsAsync(response);

        //Act
        var result = await _prodController.DeleteProduct(id.ToString()).ConfigureAwait(false);

        //Assert
        /*Assert.NotNull(result);*/
        result.Should().NotBeNull(); //fluent
        result.Should().BeAssignableTo<NoContentResult>();
 
        _serviceMock.Verify(x => x.DeleteProductAsync(id), Times.AtLeastOnce());
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnBadRequest_WhenRequestInvalid()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        ProductResult response = new()
        {
            Result = false
        };

        _serviceMock.Setup(x => x.DeleteProductAsync(id)).ReturnsAsync(response);

        //Act
        var result = await _prodController.DeleteProduct(id.ToString()).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
        _serviceMock.Verify(x => x.DeleteProductAsync(id), Times.AtLeastOnce());

    }



}

