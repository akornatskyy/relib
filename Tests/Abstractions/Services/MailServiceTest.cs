using System;
using System.Net.Mail;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class MailServiceTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void MergeTemplate_Template_IsNull()
        {
            // Arrange
            var mailService = new MailService();
            string template = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => mailService.MergeTemplate(template));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void MergeTemplate_No_Pairs()
        {
            // Arrange
            var mailService = new MailService();
            var template = "Hello <%= UserName%>!";

            // Act
            var result = mailService.MergeTemplate(template);

            // Assert
            Assert.Equal(template, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void MergeTemplate_Incomplete_Pair()
        {
            // Arrange
            var mailService = new MailService();
            var template = "Hello <%= UserName%>!";

            // Act
            var result = mailService.MergeTemplate(template, "UserName");

            // Assert
            Assert.Equal("Hello N/A!", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void MergeTemplate()
        {
            // Arrange
            var mailService = new MailService();
            var template = "Hello <%= UserName%>! Life is <%= Mood%>.";

            // Act
            var result = mailService.MergeTemplate(template, 
                "UserName", "World",
                "Mood", "good");

            // Assert
            Assert.Equal("Hello World! Life is good.", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Message_IsNull()
        {
            // Arrange
            var mailService = new MailService();
            MailMessage message = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => mailService.Send(message));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_No_Recipients()
        {
            // Arrange
            var mailService = new MockMailService();
            MailMessage message = new MailMessage();

            // Act
            Assert.Throws<ArgumentException>(() => mailService.Send(message));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Message_Has_Recipient()
        {
            // Arrange            
            var mailService = new MockMailService();
            MailMessage message = new MailMessage();
            message.To.Add("someone@test.com");

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal("someone@test.com", message.To[0].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Message_Has_Recipient_And_Recipients()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "noreply@test.com" };
            MailMessage message = new MailMessage();
            message.To.Add("someone@test.com");

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(2, message.To.Count);
            Assert.Equal("someone@test.com", message.To[0].Address);
            Assert.Equal("noreply@test.com", message.To[1].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_Recipients()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com", "noreply@test.com" };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(2, message.To.Count);
            Assert.Equal("someone@test.com", message.To[0].Address);
            Assert.Equal("noreply@test.com", message.To[1].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_Recipients_But_Empty()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com", string.Empty };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal("someone@test.com", message.To[0].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_Recipients_But_All_Empty()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { string.Empty, string.Empty };
            MailMessage message = new MailMessage();

            // Act
            Assert.Throws<ArgumentException>(() => mailService.Send(message));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_CarbonCopies()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com" };
            mailService.CarbonCopies = new[] { "u1@test.com", "u2@test.com" };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal(2, message.CC.Count);
            Assert.Equal("u1@test.com", message.CC[0].Address);
            Assert.Equal("u2@test.com", message.CC[1].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_CarbonCopies_But_Empty()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com" };
            mailService.CarbonCopies = new[] { string.Empty, "u1@test.com" };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal(1, message.CC.Count);
            Assert.Equal("u1@test.com", message.CC[0].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_BlindCarbonCopies()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com" };
            mailService.BlindCarbonCopies = new[] { "u1@test.com", "u2@test.com" };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal(0, message.CC.Count);
            Assert.Equal(2, message.Bcc.Count);
            Assert.Equal("u1@test.com", message.Bcc[0].Address);
            Assert.Equal("u2@test.com", message.Bcc[1].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Has_BlindCarbonCopies_But_Empty()
        {
            // Arrange            
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com" };
            mailService.BlindCarbonCopies = new[] { string.Empty, "u2@test.com" };
            MailMessage message = new MailMessage();

            // Act
            mailService.Send(message);

            // Assert
            Assert.Equal(1, message.To.Count);
            Assert.Equal(0, message.CC.Count);
            Assert.Equal(1, message.Bcc.Count);
            Assert.Equal("u2@test.com", message.Bcc[0].Address);
            Assert.True(mailService.WaitHandle.WaitOne());
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException))]
        [InlineData(typeof(SmtpException))]
        [Trait(Constants.TraitNames.Services, "MailService")]
        public static void Send_Throws_Exception(Type type)
        {
            // Arrange            
            var message = new MailMessage();
            var mailService = new MockMailService();
            mailService.Recipients = new[] { "someone@test.com" };
            mailService.DispatchMessageThrows = (Exception)Activator.CreateInstance(type);

            // Act
            Assert.DoesNotThrow(() => mailService.Send(message));

            // Assert
            Assert.False(mailService.WaitHandle.WaitOne(100));
            Assert.True(mailService.ExceptionHandled);
        }
    }
}
