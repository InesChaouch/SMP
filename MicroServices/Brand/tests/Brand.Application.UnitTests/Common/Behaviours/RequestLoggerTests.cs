﻿//using SMP.Application.Common.Behaviours;
//using SMP.Application.Common.Interfaces;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using SMP.Application.PostContent.Commands.GenerateContent;

//namespace SMP.Application.UnitTests.Common.Behaviours;

//public class RequestLoggerTests
//{
//    private Mock<ILogger<CreateBrandCommand>> _logger = null!;
//    private Mock<IUser> _user = null!;
//    private Mock<IIdentityService> _identityService = null!;

//    [SetUp]
//    public void Setup()
//    {
//        _logger = new Mock<ILogger<CreateBrandCommand>>();
//        _user = new Mock<IUser>();
//        _identityService = new Mock<IIdentityService>();
//    }

//    [Test]
//    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
//    {
//        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

//        var requestLogger = new LoggingBehaviour<CreateBrandCommand>(_logger.Object, _user.Object, _identityService.Object);

//        await requestLogger.Process(new CreateBrandCommand { ListId = 1, Title = "title" }, new CancellationToken());

//        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
//    }

//    [Test]
//    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
//    {
//        var requestLogger = new LoggingBehaviour<CreateBrandCommand>(_logger.Object, _user.Object, _identityService.Object);

//        await requestLogger.Process(new CreateBrandCommand { ListId = 1, Title = "title" }, new CancellationToken());

//        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
//    }
//}
