using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PaymentService.Data;
using PaymentService.Events;
using PaymentService.Services;
using PaymentService.UseCases.Commands;
using PaymentService.Models;

namespace PaymentService.Tests.UseCases
{
    [TestFixture]
    public class UpdatePaymentStatusCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldUpdateStatus_AndSendToKafka_WhenSuccessIsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PaymentDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Payments_Db")
                .Options;
            using var dbContext = new PaymentDbContext(options);

            dbContext.Payments.Add(new Payment { Id = 1, OrderId = 100, Price = 500, Status = false });
            await dbContext.SaveChangesAsync();


            var mockKafkaProducer = new Mock<IKafkaProducerService>();

            mockKafkaProducer
                .Setup(k => k.SendPaymentEventAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdatePaymentStatusCommandHandler(dbContext, mockKafkaProducer.Object);

            var command = new UpdatePaymentStatusCommand { PaymentId = 1, Status = true };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.That(result, Is.True);

            var paymentInDb = await dbContext.Payments.FindAsync(1L);
            Assert.That(paymentInDb!.Status, Is.True);

            mockKafkaProducer.Verify(
                k => k.SendPaymentEventAsync("payment_events", It.IsAny<PaymentCompletedEvent>()),
                Times.Once);
        }
    }
}
