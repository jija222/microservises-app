using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OrderService.Clients;
using OrderService.Data;
using OrderService.UseCases.Commands;

namespace OrderService.Tests
{
    [TestFixture]
    public class CreateOrderCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldCreateOrder_AndCallPaymentService() {
            //Arrange
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Orders_Db")
                .Options;
            using var dbContext = new OrderDbContext(options);

            var mockPaymentClient = new Mock<IPaymentClient>();
            mockPaymentClient.Setup(client => client.CreatePaymentAsync(It.IsAny<PaymentCreateRequest>()))
                .ReturnsAsync(new object());

            var mockLogger = new Mock<ILogger<CreateOrderCommandHandler>>();

            var handler = new CreateOrderCommandHandler(dbContext, mockPaymentClient.Object, mockLogger.Object);

            var command = new CreateOrderCommand
            {
                ProductId = 105,
                Quantity = 2,
                Price = 1000,
                ClientEmail = "test@mail.ru",
                PhoneNumber = "12345"
            };

            //Act
            var resultId = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.That(resultId, Is.GreaterThan(0));
            var orderInDb = await dbContext.Orders.FirstOrDefaultAsync();
            Assert.That(orderInDb, Is.Not.Null);
            Assert.That(orderInDb.ProductId, Is.EqualTo(105));
            Assert.That(orderInDb.ClientEmail, Is.EqualTo("test@mail.ru"));

            mockPaymentClient.Verify(
                client => client.CreatePaymentAsync(It.IsAny<PaymentCreateRequest>()),
                Times.Once);
        }
    }
}