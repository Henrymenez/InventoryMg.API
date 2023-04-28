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

    public class AuthenticationControllerTest
    {
    private readonly IFixture _fixture;
    private readonly Mock<IAuthenticationService> _authMock;
    private readonly AuthenticationController _authController;

    public AuthenticationControllerTest()
    {
        _fixture = new Fixture();
        _authMock = _fixture.Freeze<Mock<IAuthenticationService>>();
        _authController = new AuthenticationController(_authMock.Object);

    }


    [Fact]
    public async Task Register_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange

        //Act 

        //Arrset

    }
}

