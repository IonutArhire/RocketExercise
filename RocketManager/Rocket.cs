namespace RocketExercise;

public class Rocket
{
    public Rocket(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public int Id { get; }
    public int X { get; set; }
    public int Y { get; set; }
};