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
    public class SoldierLocationServiceTests
    {
        SoldierLocationService _soldierLocationService;
        Mock<ISoldierLocationRepository> _soldierLocationRepositoryMock = new();
        Mock<ITimeProvider> _timeProviderMock = new();
        Fixture _fixture = new();

        public SoldierLocationServiceTests()
        {
            _soldierLocationService = new SoldierLocationService(
                _soldierLocationRepositoryMock.Object,
                _timeProviderMock.Object);

            //Task<IEnumerable<GetLocationsInMapResponseDTO>> GetLocationsInMapAsync(LocationDTO upperLeftLocation, LocationDTO bottomRightLocation);

        }

        [TestMethod]
        public async Task UpdateSoldierLocationAsync_InvalidParameters_Throws()
        {
            // Act
            var result = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => 
                _soldierLocationService.UpdateSoldierLocationAsync(null));

            // Assert
            result.Message.Should().Contain("soldierLocationDTO");
        }

        [TestMethod]
        public async Task UpdateSoldierLocationAsync_ValidParameters_CallsRepository()
        {
            // Arrange

            _timeProviderMock.Setup(x => x.GetUTCNow()).Returns(new DateTime(2001, 6, 1));

            var request = _fixture.Build<UpdateSoldierLocationRequestDTO>()
                .With(x => x.Location, new LocationDTO(10, 15))
                .With(x => x.MovementDate, new DateTime(1945, 6, 1))
                .Create();

            // Act
            await _soldierLocationService.UpdateSoldierLocationAsync(request);

            // Assert
            _soldierLocationRepositoryMock.Verify(
                x => x.UpdateSoldierLocationAsync(
                    It.Is<SoldierLocation>(x =>
                        x.SoldierId == request.SoldierId &&
                        x.Location.Latitude == request.Location.Latitude &&
                        x.Location.Longitude == request.Location.Longitude &&
                        x.MovementDate == request.MovementDate &&
                        x.SourceTypeId == request.SourceType)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetSoldierCurrenLocationAsync_RepositoryReturnsNUll_ReturnsNull()
        {
            // Arrange
            _soldierLocationRepositoryMock
                .Setup(x => x.GetSoldierCurrenLocationAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SoldierLocation)null);

            // Act
            var result = await _soldierLocationService.GetSoldierCurrenLocationAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetSoldierInfoWithLocationAsync_RepositoryReturnsSoldier_ReturnsSoldier()
        {
            // Arrange
            var soldierLocation = _fixture.Create<SoldierLocation>();

            _soldierLocationRepositoryMock
                .Setup(x => x.GetSoldierCurrenLocationAsync(It.IsAny<Guid>()))
                .ReturnsAsync(soldierLocation);

            // Act
            var result = await _soldierLocationService.GetSoldierCurrenLocationAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(soldierLocation.Location);
        }

        [TestMethod]
        public async Task GetLocationsInMapAsync_InvalidParameters_Throws()
        {
            // Act
            var result = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                _soldierLocationService.GetLocationsInMapAsync(null, null));

            // Assert
            result.Message.Should().Contain("upperLeftLocation");
        }

        [TestMethod]
        public async Task GetLocationsInMapAsync_ValidParameters_ReturnsData()
        {
            // Arrange
            var locations = _fixture.CreateMany<SoldierLocation>();
            _soldierLocationRepositoryMock
                .Setup(x => x.GetLocationsInMapAsync(It.IsAny<Location>(), It.IsAny<Location>()))
                .ReturnsAsync(locations);

            // Act
            var result = await _soldierLocationService.GetLocationsInMapAsync(
                new LocationDTO(10, 11),
                new LocationDTO(33, 45));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                locations, 
                opt => opt
                    .Including(z => z.Location)
                    .Including(z => z.SoldierId));
        }

    }
}