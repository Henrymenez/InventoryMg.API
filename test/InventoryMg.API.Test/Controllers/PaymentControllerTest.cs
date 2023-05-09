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
    public class PaymentControllerTest
    {
    private readonly IFixture _fixture;
    private readonly Mock<IPaymentService> _paymentMock;
    private readonly PaymentController _paymentController;

    public PaymentControllerTest()
    {
        _fixture = new Fixture();
        _paymentMock = _fixture.Freeze<Mock<IPaymentService>>();
        _paymentController = new PaymentController(_paymentMock.Object);

    }


    [Fact]
    public async Task UserProfile_ShouldReturnOkResponse_WhenDataIsValid()
    {
        //Arrange

        //Act 

        //Arrset

    }
}

