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

    public class UserControllerTest
    {
    private readonly IFixture _fixture;
    private readonly Mock<IUserService> _userMock;
    private readonly UserController _userController;

    public UserControllerTest()
    {
        _fixture = new Fixture();
        _userMock = _fixture.Freeze<Mock<IUserService>>();
        _userController = new UserController(_userMock.Object);

    }


    [Fact]
    public async Task UserProfile_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange

        //Act 

        //Arrset

    }
}

