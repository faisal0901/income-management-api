using System.Linq.Expressions;
using budget_management_api.Dtos;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;
using budget_management_api.Security;
using budget_management_api.Services;
using budget_management_api.Services.Impl;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace budget_management_api.test.Services;

public class AuthServiceTest
{
    private readonly Mock<IRepository<User>> _mockRepository;
    private readonly Mock<IPersistence> _mockPersistence;
    private readonly Mock<IJwtUtil> _jwtUtil;
    private readonly Mock<IWalletService> _walletService;
    private readonly Mock<ITokenService> _tokenService;
    private User _user;
    
    private IAuthService _authService;
    
    public AuthServiceTest(Mock<ITokenService> tokenService)
    {
        _tokenService = tokenService;
        _mockRepository = new Mock<IRepository<User>>();
        _mockPersistence = new Mock<IPersistence>();
        _jwtUtil = new Mock<IJwtUtil>();
        _walletService = new Mock<IWalletService>();
        _authService = new AuthService(_mockRepository.Object, _mockPersistence.Object, _jwtUtil.Object, _walletService.Object,_tokenService.Object);
        _user = new User
        {
            Id = Guid.NewGuid(),
            Email = "unit@testing.com",
            Password = "password123",
            PhoneNumber = "08123456789",
            Role = ERole.Free,
        };
    }

    [Fact]
    public async Task Should_ReturnUserEmail_Where_LoadByEmail()
    {
        // Arrange
        var email = "unit@testing.com";
        _mockRepository.Setup(repo => repo.Find(u => u.Email.Equals(email))).ReturnsAsync(_user);
        var expected = _user.Email;

        // Act
        var result = await _authService.LoadByEmail(email);
        
        // Assert
        Assert.Equal(expected, result.Email);
    }

    [Fact]
    public async Task Should_ReturnNotFoundException_When_LoadByEmailWhereIsNull()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _authService.LoadByEmail(String.Empty));
    }

    [Fact]
    public async Task Should_ReturnEmail_When_LoadRegisterEmailWhenIsNull()
    {
        var email = "test@unit.com";
        _mockRepository.Setup(repo => repo.Find(u => u.Email.Equals(_user.Email))).ReturnsAsync(_user);

        var result = await _authService.LoadRegisterEmail(email);
        
        Assert.NotEqual(result, _user.Email);
    }

    [Fact]
    public async Task Should_ReturnUnauthorizedException_When_LoadRegisterEmailIsNotNull()
    {
        var email = "test@unit.com";
        _mockRepository.Setup(repo => repo.Find(u => u.Email.Equals(email))).ReturnsAsync(_user);
        await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.LoadRegisterEmail(email));
    }
    
    [Fact]
    public async Task Should_ReturnsRegisterResponse_When_RegisterFreeSuccess()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password",
            PhoneNumber = "1234567890"
        };
        
        _mockRepository.Setup(repo => repo.Save(It.IsAny<User>()))
            .ReturnsAsync(new User
            {
                Email = request.Email,
                Role = ERole.Free
            });
        
        _mockPersistence.Setup(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()))
            .Callback<Func<Task<RegisterResponse>>>(func => func())
            .ReturnsAsync(new RegisterResponse
            {
                Email = request.Email,
                Role = ERole.Free.ToString()
            });

        // Act
        var result = await _authService.RegisterFree(request);

        // Assert
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(ERole.Free.ToString(), result.Role);

        _mockRepository.Verify(repo => repo.Save(It.IsAny<User>()), Times.Once());
        _mockPersistence.Verify(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()), Times.Once());
    }
    
    [Fact]
    public async Task Should_ReturnsRegisterResponse_When_RegisterPremiumSuccess()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password",
            PhoneNumber = "1234567890"
        };
        
        _mockRepository.Setup(repo => repo.Save(It.IsAny<User>()))
            .ReturnsAsync(new User
            {
                Email = request.Email,
                Role = ERole.Premium
            });
        
        _mockPersistence.Setup(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()))
            .Callback<Func<Task<RegisterResponse>>>(func => func())
            .ReturnsAsync(new RegisterResponse
            {
                Email = request.Email,
                Role = ERole.Premium.ToString()
            });

        // Act
        var result = await _authService.RegisterPremium(request);

        // Assert
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(ERole.Premium.ToString(), result.Role);

        _mockRepository.Verify(repo => repo.Save(It.IsAny<User>()), Times.Once());
        _mockPersistence.Verify(p => p.ExecuteTransactionAsync(It.IsAny<Func<Task<RegisterResponse>>>()), Times.Once());
    }

    [Fact]
    public async Task Should_ReturnLoginResponse_When_LoginIsValid()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password"
        };

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = ERole.Free
        };
        
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        var expectedToken = "expected-token";
        _jwtUtil.Setup(util => util.GenerateToken(user)).Returns(expectedToken);

        // Act
        var result = await _authService.Login(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(user.Role.ToString(), result.Role);
        Assert.Equal(expectedToken, result.Token);
    }

    [Fact]
    public async Task Should_ReturnUnauthorized_When_LoginIsNotValid()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password"
        };

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword("WrongPassword"),
            Role = ERole.Free
        };
        
        _mockRepository.Setup(repo => repo.Find(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.Login(request));
    }

}