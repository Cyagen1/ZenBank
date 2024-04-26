using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZenCore.Controllers;
using ZenCore.DataAccess;
using ZenCore.Entities;
using ZenCore.Models;

namespace ZenCore.Tests
{
    public class UsersControllerTests : IDisposable
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly UsersController _sut;
        public UsersControllerTests()
        {
            _sut = new(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ShouldGetAllUsers()
        {
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<User>());

            var result = await _sut.GetAllUsersAsync();

            _userRepositoryMock.Verify(x => x.GetAllUsersAsync(), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ShouldGetUserById()
        {
            var userId = new Guid();
            var user = new User { Id = userId, Name = "Test", Email = "test@mail.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(new UserDto(user.Name, user.Email));

            var result = await _sut.GetUserByIdAsync(userId);

            _userRepositoryMock.Verify(x => x.GetUserById(It.Is<Guid>(g => g.Equals(userId))), Times.Once);
            _mapperMock.Verify(x => x.Map<UserDto>(It.Is<User>(u => u.Name == user.Name && u.Email == user.Email)), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userDto = Assert.IsType<UserDto>(okResult.Value);

            Assert.NotNull(okResult);
            Assert.Equal(user.Name, userDto.Name);
            Assert.Equal(user.Email, userDto.Email);
        }

        [Fact]
        public async Task ShouldCreateNewUser()
        {
            var userDto = new UserDto("Test", "test@mail.com");
            var user = new User { Id = new Guid(), Name = "Test", Email = "test@mail.com" };
            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);

            var result = await _sut.CreateUserAsync(userDto);

            _userRepositoryMock.Verify(x => x.CreateUserAsync(It.Is<User>(x => x.Name == userDto.Name && x.Email == userDto.Email)), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(It.Is<UserDto>(u => u.Name == user.Name && u.Email == user.Email)), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var userResult = Assert.IsType<Guid>(okResult.Value);
            Assert.NotNull(okResult);
            Assert.Equal(userResult, user.Id);
        }

        [Fact]
        public async Task ShouldDeleteUser()
        {
            var userId = new Guid();

            var result = await _sut.DeleteUserAsync(userId);

            _userRepositoryMock.Verify(x => x.DeleteUserAsync(It.Is<Guid>(g => g.Equals(userId))), Times.Once);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task ShouldFindAndUpdateUser()
        {
            var userId = new Guid();
            var userDto = new UserForUpdateDto(userId, "UpdatedTest", "UpdatedTest@mail.com");
            var user = new User { Id = userId, Name = "Test", Email = "Test@mail.com" };
            var updatedUser = new User { Id = userId, Name = userDto.Name, Email = userDto.Email };
            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(user)).ReturnsAsync(updatedUser);

            var result = await _sut.UpdateUserAsync(userDto);

            _mapperMock.Verify(x => x.Map<User>(It.Is<UserForUpdateDto>(u => u.Id == userId 
            && u.Name == updatedUser.Name 
            && u.Email == updatedUser.Email)), Times.Once);

            _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.Is<User>(u => u.Id == userId
            && u.Name == user.Name
            && u.Email == user.Email)), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var userResult = Assert.IsType<User>(okResult.Value);

            Assert.NotNull(okResult);
            Assert.Equal(updatedUser.Name, userResult.Name);
        }

        [Fact]
        public async Task ShouldNotFindAndUpdateUser()
        {
            var userId = new Guid();
            var userDto = new UserForUpdateDto(userId, "UpdatedTest", "UpdatedTest@mail.com");
            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(new User());
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync((User)null);

            var result = await _sut.UpdateUserAsync(userDto);

            _mapperMock.Verify(x => x.Map<User>(It.IsAny<UserForUpdateDto>()), Times.Once);

            _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Once);

            Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            _userRepositoryMock.VerifyNoOtherCalls();
            _mapperMock.VerifyNoOtherCalls();
        }
    }
}
