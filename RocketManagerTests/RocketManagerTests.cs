using FluentAssertions;
using RocketExercise;
using Xunit;

namespace RocketManagerTests;

public class RocketManagerTests
{
    private readonly RocketManager _rocketManager;

    public RocketManagerTests()
    {
        _rocketManager = new RocketManager(5, 5, 5);
    }

    [Theory]
    [InlineData(-1, 5, 5)]
    [InlineData(5, -1, 5)]
    [InlineData(5, 5, 0)]
    [InlineData(101, 5, 5)]
    [InlineData(5, 101, 5)]
    [InlineData(99, 5, 5)]
    [InlineData(5, 99, 5)]
    public void Given_InvalidLandingPlatformSpecification_When_CreatingARocketManager_Then_Throw(int x, int y, int size)
    {
        // arrange
        var act = () => new RocketManager(x, y, size);

        // act & assert
        act.Should().Throw<LandingPlatformSpecificationException>();
    }

    [Theory]
    [InlineData(4, 5)]
    [InlineData(5, 4)]
    [InlineData(11, 5)]
    [InlineData(5, 11)]
    public void Given_OutOfPlatformTrajectory_When_CheckLanding_Then_ReturnOutOfPlatform(int x, int y)
    {
        // act
        var result = _rocketManager.CheckLanding(1, x, y);

        // assert
        result.Should().Be(LandingStatus.OutOfPlatform);
    }

    [Fact]
    public void
        Given_SecondTrajectoryInTheVicinityOfTheFirstOne_When_CheckLandingWithTheSameRocket_Then_ShouldNotClashWithItself()
    {
        // act
        var firstCheck = _rocketManager.CheckLanding(1, 5, 5);
        var secondCheck = _rocketManager.CheckLanding(1, 6, 6);

        // assert
        firstCheck.Should().Be(LandingStatus.OkForLanding);
        secondCheck.Should().Be(LandingStatus.OkForLanding);
    }

    [Theory]
    [InlineData(6, 6)]
    [InlineData(6, 7)]
    [InlineData(6, 8)]
    [InlineData(7, 6)]
    [InlineData(7, 7)]
    [InlineData(7, 8)]
    [InlineData(8, 6)]
    [InlineData(8, 7)]
    [InlineData(8, 8)]
    public void Given_SecondRocket_When_CheckLandingInTheVicinityOfTheFirstRocket_Then_ReturnClash(int x, int y)
    {
        // arrange
        var firstRocketCheck = _rocketManager.CheckLanding(1, 7, 7);

        // act
        var secondRocketCheck = _rocketManager.CheckLanding(2, x, y);

        // assert
        firstRocketCheck.Should().Be(LandingStatus.OkForLanding);
        secondRocketCheck.Should().Be(LandingStatus.Clash);
    }

    [Fact]
    public void Given_SecondRocket_When_CheckLandingInTheVicinityOfTheFirstRocketsUpdatedTrajectory_Then_ReturnClash()
    {
        // arrange
        Given_SecondTrajectoryInTheVicinityOfTheFirstOne_When_CheckLandingWithTheSameRocket_Then_ShouldNotClashWithItself();

        // act
        var secondRocketCheck = _rocketManager.CheckLanding(2, 7, 7);

        // assert
        secondRocketCheck.Should().Be(LandingStatus.Clash);
    }
}