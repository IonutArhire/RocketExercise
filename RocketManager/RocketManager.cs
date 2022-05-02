namespace RocketExercise;

public class RocketManager
{
    private const int LandingAreaSize = 100;

    private readonly LandingPlatform _landingPlatform;
    private readonly List<Rocket> _rockets = new();

    public RocketManager(int x, int y, int size)
    {
        if (x < 0 ||
            y < 0 ||
            size < 1 ||
            x > LandingAreaSize ||
            y > LandingAreaSize ||
            x + size > LandingAreaSize ||
            y + size > LandingAreaSize)
        {
            throw new LandingPlatformSpecificationException("Landing Platform is outside the boundaries!");
        }

        _landingPlatform = new LandingPlatform(x, y, size);
    }

    public LandingStatus CheckLanding(int rocketId, int x, int y)
    {
        if (IsOutOfPlatform(x, y)) return LandingStatus.OutOfPlatform;
        if (IsClash(rocketId, x, y)) return LandingStatus.Clash;

        LogLastRocketTrajectory(rocketId, x, y);
        return LandingStatus.OkForLanding;
    }

    private bool IsOutOfPlatform(int x, int y)
    {
        return x < _landingPlatform.X ||
               y < _landingPlatform.Y ||
               x > _landingPlatform.X + _landingPlatform.Size ||
               y > _landingPlatform.Y + _landingPlatform.Size;
    }

    private bool IsClash(int rocketId, int x, int y)
    {
        return _rockets.Any(rocket => rocket.Id != rocketId && IsRocketInVicinity(rocket, x, y));
    }

    private void LogLastRocketTrajectory(int rocketId, int x, int y)
    {
        var rocket = _rockets.SingleOrDefault(rocket => rocket.Id == rocketId);
        if (rocket == null)
        {
            _rockets.Add(new Rocket(rocketId, x, y));
        }
        else
        {
            rocket.X = x;
            rocket.Y = y;
        }
    }

    private static bool IsRocketInVicinity(Rocket rocket, int x, int y)
    {
        return rocket.X >= x - 1 &&
               rocket.X <= x + 1 &&
               rocket.Y >= y - 1 &&
               rocket.Y <= y + 1;
    }
}