using Xunit;
using AutoFixture;
using FluentAssertions;
using Moq;
using InventoryMg.DAL.Repository;
using InventoryMg.DAL.Entities;

namespace InventoryMg.BLL.Test.Services;
    public class SalesServicesTest
    {

    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mock<IRepository<Sale>> _saleRepo;

    [Fact]
    public void Test1()
    {

    }
}

