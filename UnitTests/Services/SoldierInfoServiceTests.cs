using AutoFixture;
using FluentAssertions;
using Moq;
using Services;
using Services.DTOs;
using Services.Models;
using Services.Projections;

namespace UnitTests.Services
{
    [TestClass]
    public class SoldierInfoServiceTests
    {
        SoldierInfoService _soldierInfoService;
        Mock<ISoldierInfoRepository> _soldierInfoRepositoryMock = new();
        Mock<ISoldierLocationService> _soldierLocationServiceMock = new();
        Fixture _fixture = new();

        public SoldierInfoServiceTests()
        {
            _soldierInfoService = new SoldierInfoService(
                _soldierInfoRepositoryMock.Object,
                _soldierLocationServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_InvalidParameters_Throws()
        {
            // Act
            var result = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => 
                _soldierInfoService.GetAllAsync(0, 1));

            // Assert
            result.Message.Should().Be("Must be greater than 0 (Parameter 'pageSize')");
        }

        [TestMethod]
        public async Task GetAllAsync_ValidParameters_ReturnsSoldierInfo()
        {
            // Arrange
            const int pageSize = 3;
            var data = _fixture.CreateMany<Soldier>(pageSize);
            var location1 = new LocationDTO(1, 1);
            var location2 = new LocationDTO(2, 2);
            LocationDTO location3 = null;

            _soldierInfoRepositoryMock
                .Setup(x => x.GetAllAsync(pageSize, It.IsAny<int>(), It.IsAny<GetSoldierProjection>()))
                .ReturnsAsync(data);

            _soldierLocationServiceMock
                .SetupSequence(x => x.GetSoldierCurrenLocationAsync(It.IsAny<Guid>()))
                .ReturnsAsync(location1)
                .ReturnsAsync(location2)
                .ReturnsAsync(location3);

            // Act
            var result = await _soldierInfoService.GetAllAsync(pageSize, 1);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(pageSize);
            result.Select(r => r.SoldierId).Should()
                .BeEquivalentTo(data.Select(d => d.Id));
            result.Select(r => r.SoldierName).Should()
                .BeEquivalentTo(data.Select(d => d.Name));
            result.Select(r => r.CountryName).Should()
                .BeEquivalentTo(data.Select(d => d.Country?.Name ?? ""));
            result.Select(r => r.Location).Should()
                .BeEquivalentTo(new[] { location1, location2, location3 });
        }

        [TestMethod]
        public async Task GetSoldierInfoWithLocationAsync_RepositoryReturnsNUll_ReturnsNull()
        {
            // Arrange
            _soldierInfoRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<GetSoldierProjection>()))
                .ReturnsAsync((Soldier)null);

            // Act
            var result = await _soldierInfoService.GetSoldierInfoWithLocationAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetSoldierInfoWithLocationAsync_RepositoryReturnsSoldier_ReturnsSoldier()
        {
            // Arrange
            var soldier = _fixture.Create<Soldier>();
            var location = _fixture.Create<LocationDTO>();

            _soldierInfoRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<GetSoldierProjection>()))
                .ReturnsAsync(soldier);

            _soldierLocationServiceMock
                .Setup(x => x.GetSoldierCurrenLocationAsync(It.IsAny<Guid>()))
                .ReturnsAsync(location);

            // Act
            var result = await _soldierInfoService.GetSoldierInfoWithLocationAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.SoldierId.Should().Be(soldier.Id);
            result.SoldierName.Should().Be(soldier.Name);
            result.RankDescription.Should().Be(soldier.Rank?.Description ?? string.Empty);
            result.CountryName.Should().Be(soldier.Country?.Name ?? string.Empty);
            result.SensorTypeDescription.Should().Be(soldier.SensorType?.Description ?? string.Empty);
            result.TrainingInfo.Should().Be(soldier.TrainingInfo);
            result.SensorName.Should().Be(soldier.SensorName);
            result.Location.Should().BeEquivalentTo(location);
        }
    }
}