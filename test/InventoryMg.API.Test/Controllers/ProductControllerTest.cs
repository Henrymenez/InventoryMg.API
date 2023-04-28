using AutoFixture;
using InventoryMg.API.Controllers;
using InventoryMg.BLL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMg.API.Test.Controllers;

    public class ProductControllerTest
    {
    private readonly IFixture _fixture;
    private readonly Mock<IProductService> _prodMock;
    private readonly ProductController _prodController;

    public ProductControllerTest()
    {
        _fixture = new Fixture();
        _prodMock = _fixture.Freeze<Mock<IProductService>>();
        _prodController = new ProductController(_prodMock.Object);

    }


    [Fact]
    public async Task GetProducts_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange

        //Act 

        //Arrset

    }
}

