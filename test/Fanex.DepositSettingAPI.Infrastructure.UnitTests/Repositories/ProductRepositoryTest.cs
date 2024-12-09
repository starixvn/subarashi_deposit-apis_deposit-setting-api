namespace Fanex.DepositSettingAPI.Infrastructure.UnitTests.Repositories;

using System.Collections;
using AutoFixture;
using Constants;
using Domain.Entities;
using Domain.Params;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Repositories;
using NSubstitute;

[Collection("DbFactoryFixture")]
public class ProductRepositoryTest(DbFactoryFixture dbFactoryFixture) : RepositoryBaseTest(dbFactoryFixture)
{
    private readonly IProductRepository _productRepository = new ProductRepository(dbFactoryFixture);

    [Fact]
    public async Task GetProductStatusAsync_Should_SetParameterValueCorrect()
    {
        // Arrange
        var param = Fixture.Create<GetProductStatusParam>();

        // Action
        await _productRepository.GetProductStatusAsync(param);

        // Assert
        ObjectDb.Received(1).SetParameterValue(DbParam.AgentId, param.AgentId);
        ObjectDb.Received(1).SetParameterValue(DbParam.ProductId, param.ProductId);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetProductStatusAsync_Should_ReturnCorrectData(bool isEnabled)
    {
        // Arrange
        var param = Fixture.Create<GetProductStatusParam>();

        var expected = new ProductStatus
        {
            AgentId = param.AgentId,
            ProductId = param.ProductId,
            IsEnabled = isEnabled
        };

        ObjectDb.GetOutputParams().Returns(new Hashtable
        {
            { DbParam.IsEnabled, isEnabled }
        });

        // Action
        var result = await _productRepository.GetProductStatusAsync(param);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}