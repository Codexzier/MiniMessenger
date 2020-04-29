using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniMessenger.Components.Data;
using MiniMessenger.Components.Service;
using System.Linq;

namespace MiniMessenger.Test
{
    [TestClass]
    public class ServiceConnectorTest
    {
        private const long _adminUserId = 1;

        [TestMethod]
        public void ServiceConnectorGetAllUsersTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");

            // act
            var result = serviceConnector.GetAllUsers();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ServiceConnectorTrySetUsernameTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            const string username = "testUser";

            // act
            var success = serviceConnector.TrySetUsername(username, out var result);

            // assert
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);
            Assert.AreNotEqual(0, result.ID);
        }

        [TestMethod]
        public void ServiceConnectorSetUserAndGetAllUsersTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.TrySetUsername("testUser", out var testUser);

            // act
            var result = serviceConnector.GetAllUsers();

            // assert
            Assert.IsNotNull(result);
            // only admin, well it show only other not self user
            Assert.AreEqual(1, result.Length);
        }


        [TestMethod]
        public void ServiceConnectorGetMessagesTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.TrySetUsername("testUser", out var testUser);

            // act
            var result = serviceConnector.GetMessages(_adminUserId);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual("Hallo ich bin der Admin Benutzer", result.First().Text);
        }

        [TestMethod]
        public void ServiceConnectorSendMessageTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.TrySetUsername("testUser", out var testUser);

            // act
            var result = serviceConnector.SendMessage(_adminUserId, "Hallo test");

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ServiceConnectorSendMessageAndGetMessageTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.TrySetUsername("testUser", out _);

            // act
            serviceConnector.SendMessage(_adminUserId, "Hallo test");
            var result = serviceConnector.GetMessages(_adminUserId);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual("Hallo test", result.Last().Text);
        }




        [TestMethod]
        public void ServiceConnectorDeviceGetAllTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");

            // act
            var result = serviceConnector.DeviceGetAll();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ServiceConnectorDeviceGetValueTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            // it must a non exist device
            const long id = 12345678L;
            // act
            var result = serviceConnector.DeviceGetValue(id);

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ServiceConnectorDeviceSendComandValueTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.DeviceGetValue(1L);
            const long value = 234L;

            // act
            var result = serviceConnector.DeviceSendCommand(1L, 234);
            var resultValue = serviceConnector.DeviceGetValue(1L);

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, resultValue);
        }

        [TestMethod]
        public void ServiceConnectorDeviceSendComandTextTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.DeviceGetText(1L);
            string sendText = "Hello";

            // act
            var result = serviceConnector.DeviceSendCommand(1L, sendText);
            var resultText = serviceConnector.DeviceGetText(1L);

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual(sendText, resultText);
        }

        [TestMethod]
        public void ServiceConnectorDeviceSendComandGetResponseTest()
        {
            // arrange
            IServiceConnector serviceConnector = ServiceConnector.GetInstance();
            serviceConnector.SetAddress("http://localhost:5000/");
            serviceConnector.DeviceGet(1L);
            const long id = 1L;
            const long value = 456L;
            string sendText = "Hello";

            // act
            var result = serviceConnector.DeviceSendCommand(id, value, sendText);
            var resultResponse = serviceConnector.DeviceGet(id);

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual(sendText, resultResponse.Text);
            Assert.AreEqual(value, resultResponse.Value);
        }
    }
}
