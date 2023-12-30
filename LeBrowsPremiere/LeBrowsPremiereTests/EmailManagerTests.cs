using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using System.Net.Mail;

namespace LeBrowsPremiereTests
{
    public class EmailManagerTests
    {
        [Fact]
        public void CreateSmtpClient_ValidEmailConfiguration_ReturnsSmtpClient()
        {
            // Arrange
            var emailConfig = new EmailConfigurationModel
            {
                SmtpClientHost = "smtp.gmail.com",
                SmtpClientPort = 587,
                NetworkCredentialEmail = "example@gmail.com",
                NetworkCredentialPassword = "password123"
            };

            // Act
            var result = EmailManager.CreateSmtpClient(emailConfig);

            // Assert
            Assert.IsType<SmtpClient>(result);
            Assert.Equal(emailConfig.SmtpClientHost, result.Host);
            Assert.Equal(emailConfig.SmtpClientPort, result.Port);
            Assert.True(result.EnableSsl);
            Assert.False(result.UseDefaultCredentials);
            Assert.Equal(SmtpDeliveryMethod.Network, result.DeliveryMethod);
        }

        [Fact]
        public void CreateSmtpClient_NullEmailConfiguration_ThrowsArgumentNullException()
        {
            // Arrange
            EmailConfigurationModel emailConfig = null;

            // Act and Assert
            Assert.Throws<NullReferenceException>(() => EmailManager.CreateSmtpClient(emailConfig));
        }

        [Fact]
        public void CreateSmtpClient_NullNetworkCredential_ReturnsShellSmtpClient()
        {
            // Arrange
            var emailConfig = new EmailConfigurationModel
            {
                SmtpClientHost = "smtp.gmail.com",
                SmtpClientPort = 587,
                NetworkCredentialEmail = null,
                NetworkCredentialPassword = null
            };

            // Act and Assert
            Assert.NotEqual(null, EmailManager.CreateSmtpClient(emailConfig));
        }

        [Fact]
        public void CreateMessage_ValidInputs_ReturnsMailMessageObject()
        {
            // Arrange
            var emailConfiguration = new EmailConfigurationModel()
            {
                NetworkCredentialEmail = "sender@test.com",
                NetworkCredentialPassword = "password123"
            };
            var clientEmailInfo = new ClientEmailInformationModel()
            {
                Email = "receiver@test.com",
                FirstName = "John",
                LastName = "Doe",
                MessageSubject = "Test Subject",
                MessageBody = "Test Message Body"
            };

            // Act
            var result = EmailManager.CreateMessage(emailConfiguration, clientEmailInfo);

            // Assert
            Assert.IsType<MailMessage>(result);
        }

        [Fact]
        public void CreateMessage_InvalidEmailConfiguration_ThrowsArgumentException()
        {
            // Arrange
            var emailConfiguration = new EmailConfigurationModel();
            var clientEmailInfo = new ClientEmailInformationModel()
            {
                Email = "receiver@test.com",
                FirstName = "John",
                LastName = "Doe",
                MessageSubject = "Test Subject",
                MessageBody = "Test Message Body"
            };

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => EmailManager.CreateMessage(emailConfiguration, clientEmailInfo));
        }

        [Fact]
        public void CreateMessage_InvalidClientEmailInfo_ThrowsArgumentException()
        {
            // Arrange
            var emailConfiguration = new EmailConfigurationModel()
            {
                NetworkCredentialEmail = "sender@test.com",
                NetworkCredentialPassword = "password123"
            };
            var clientEmailInfo = new ClientEmailInformationModel();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => EmailManager.CreateMessage(emailConfiguration, clientEmailInfo));
        }

        [Fact]
        public void CreateMessage_ValidInputs_ReturnsMailMessageObjectWithCorrectProperties()
        {
            // Arrange
            var emailConfiguration = new EmailConfigurationModel()
            {
                NetworkCredentialEmail = "sender@test.com",
                NetworkCredentialPassword = "password123"
            };
            var clientEmailInfo = new ClientEmailInformationModel()
            {
                Email = "receiver@test.com",
                FirstName = "John",
                LastName = "Doe",
                MessageSubject = "Test Subject",
                MessageBody = "Test Message Body"
            };

            // Act
            var result = EmailManager.CreateMessage(emailConfiguration, clientEmailInfo);

            // Assert
            Assert.Equal(emailConfiguration.NetworkCredentialEmail, result.From.Address);
            Assert.Equal("LeBrows Premiere", result.From.DisplayName);
            Assert.Equal(clientEmailInfo.Email, result.To[0].Address);
            Assert.Equal($"{clientEmailInfo.FirstName} {clientEmailInfo.LastName}", result.To[0].DisplayName);
            Assert.Equal(clientEmailInfo.MessageSubject, result.Subject);
            Assert.Equal(clientEmailInfo.MessageBody, result.Body);
        }

    }
}
