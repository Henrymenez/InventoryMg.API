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

    public class RoleControllerTest
    {
    private readonly IFixture _fixture;
    private readonly Mock<IRoleService> _roleMock;
    private readonly RoleController _roleController;

    public RoleControllerTest()
    {
        _fixture = new Fixture();
        _roleMock = _fixture.Freeze<Mock<IRoleService>>();
        _roleController = new RoleController(_roleMock.Object);

    }


    [Fact]
    public async Task GetRoles_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange

        //Act 

        //Arrset

    }
}

