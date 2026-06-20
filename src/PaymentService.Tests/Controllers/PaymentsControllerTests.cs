using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PaymentService.Controllers;
using PaymentService.UseCases.Commands;

namespace PaymentService.Tests.Controllers
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new PaymentsController(_mockMediator.Object);
        }
        [Test]
        public async Task UpdateStatus_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            long testPaymentId = 1;
            bool testStatus = true;

            _mockMediator
                .Setup(m => m.Send(It.IsAny<UpdatePaymentStatusCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdatePaymentStatus(testPaymentId, testStatus);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            _mockMediator.Verify(m => m.Send(It.IsAny<UpdatePaymentStatusCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateStatus_ShouldReturnNotFound_WhenPaymentDoesNotExist()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<UpdatePaymentStatusCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdatePaymentStatus(999, true);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}
