using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OrderService.Controllers;
using OrderService.UseCases.Commands;
using OrderService.UseCases.Queries;

namespace OrderService.Tests.Controllers
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private OrdersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new OrdersController(_mockMediator.Object);
        }
        [Test]
        public async Task CreateOrder_ShouldReturnOk_WithOrderId()
        {
            // Arrange 
            var command = new CreateOrderCommand { ProductId = 1, Price = 100 };
            var expectedOrderId = 5L; 

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOrderId);

            // Act 
            var result = await _controller.CreateOrder(command);

            // Assert 
            Assert.That(result, Is.InstanceOf<OkObjectResult>());


            var okResult = (OkObjectResult)result;

            var returnedValue = okResult.Value;
            var propertyInfo = returnedValue?.GetType().GetProperty("OrderId");
            var actualOrderId = (long?)propertyInfo?.GetValue(returnedValue, null);

            Assert.That(actualOrderId, Is.EqualTo(expectedOrderId));
        }

        [Test]
        public async Task GetOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            long searchId = 99;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderResponse?)null);

            // Act
            var result = await _controller.GetOrder(searchId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetOrder_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            long searchId = 1;
            var expectedOrder = new OrderResponse { ProductId = 10, Price = 500 };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _controller.GetOrder(searchId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedOrder)); 
        }
    }
}
