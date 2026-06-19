using Microsoft.AspNetCore.SignalR;
using Moq;
using NotificationService.Hubs;
using static NotificationService.Services.INotificationDispatcher;

namespace NotificationService.Tests.Services
{
    [TestFixture]
    public class SignalRNotificationDispatcherTests
    {
        [Test]
        public async Task DispatchAsync_ShouldSendMessage_ToAllClients()
        {
            // Arrange 
            var testMessage = "{\"PaymentId\":1,\"IsSuccess\":true}";

            var mockHubContext = new Mock<IHubContext<NotificationHub>>();
            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockHubContext.Setup(x => x.Clients).Returns(mockClients.Object);
            mockClients.Setup(x => x.All).Returns(mockClientProxy.Object);

            var dispatcher = new SignalRNotificationDispatcher(mockHubContext.Object);

            //Act
            await dispatcher.DispatchAsync(testMessage, CancellationToken.None);

            // Assert
            mockClientProxy.Verify(
                client => client.SendCoreAsync(
                    "ReceiveNotification", 
                    It.Is<object[]>(args => args != null && args.Length == 1 && args[0].ToString() == testMessage),
                    It.IsAny<CancellationToken>()),
                Times.Once); 
        }
    }
}