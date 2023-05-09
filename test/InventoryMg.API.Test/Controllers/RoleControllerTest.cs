using AutoFixture;
using FluentAssertions;
using InventoryMg.API.Controllers;
using InventoryMg.BLL.DTOs.Response;
using InventoryMg.BLL.Interfaces;
using InventoryMg.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
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
    private readonly Mock<IRoleService> _serviceMock;
    private readonly RoleController _roleController;

    public RoleControllerTest()
    {
        _fixture = new Fixture();
        _serviceMock = _fixture.Freeze<Mock<IRoleService>>();
        _roleController = new RoleController(_serviceMock.Object);

    }


    [Fact]
    public async Task GetRoles_ShouldReturnOkResponse_WhenDataIsValid()
    { 
        //Arrange
        var roleMock = _fixture.Create<IEnumerable<AppRole>>();
        _serviceMock.Setup(x => x.GetAllRoles()).ReturnsAsync(roleMock);
        //Act
        var result = await _roleController.GetRoles().ConfigureAwait(false);

        //Assert
     
        result.Should().NotBeNull(); //fluent
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(roleMock.GetType());
        _serviceMock.Verify(x => x.GetAllRoles(), Times.Once());
    }
    [Fact]
    public async Task GetRoles_ShouldReturnNotFound_WhenDataIsInValid()
    {
        //Arrange
        IEnumerable<AppRole> roleMock = null;
        _serviceMock.Setup(x => x.GetAllRoles()).ReturnsAsync(roleMock);
        //Act
        var result = await _roleController.GetRoles().ConfigureAwait(false);

        //Assert

        result.Should().NotBeNull(); //fluent
        result.Should().BeAssignableTo<NotFoundResult>();
        _serviceMock.Verify(x => x.GetAllRoles(), Times.Once());
    }
}

