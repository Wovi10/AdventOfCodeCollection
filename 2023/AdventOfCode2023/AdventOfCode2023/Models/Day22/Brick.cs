namespace AdventOfCode2023_1.Models.Day22;

public class Brick
{
    public Brick(string input)
    {
        var parts = input.Split('~');
        StartCube = new Cube
        {
            X = int.Parse(parts[0].Split(',')[0]),
            Y = int.Parse(parts[0].Split(',')[1]),
            Z = int.Parse(parts[0].Split(',')[2])
        };
        EndCube = new Cube
        {
            X = int.Parse(parts[1].Split(',')[0]),
            Y = int.Parse(parts[1].Split(',')[1]),
            Z = int.Parse(parts[1].Split(',')[2])
        };

        Cubes = new List<Cube>();

        IsValid = StartCube.Z != 0 && EndCube.Z != 0;

        if (!IsValid)
            return;

        if (StartCube.X != EndCube.X)
        {
            for (var x = StartCube.X; x <= EndCube.X; x++)
            {
                Cubes.Add(new Cube {X = x, Y = StartCube.Y, Z = StartCube.Z});
            }

            return;
        }

        if (StartCube.Y != EndCube.Y)
        {
            for (var y = StartCube.Y; y <= EndCube.Y; y++)
            {
                Cubes.Add(new Cube {X = StartCube.X, Y = y, Z = StartCube.Z});
            }

            return;
        }

        for (var z = StartCube.Z; z <= EndCube.Z; z++)
        {
            Cubes.Add(new Cube {X = StartCube.X, Y = StartCube.Y, Z = z});
        }
    }

    public Cube StartCube { get; set; }
    public Cube EndCube { get; set; }

    public List<Cube> Cubes { get; set; }

    public bool IsValid { get; set; }

    public int LowestX => Cubes.Min(c => c.X);
    public int LowestY => Cubes.Min(c => c.Y);
    public int LowestZ => Cubes.Min(c => c.Z);
    public int HighestZ => Cubes.Max(c => c.Z);

    public bool CanMoveDown => LowestZ > 1;

    public void MoveDown()
    {
        foreach (var cube in Cubes) 
            cube.Z--;
        
        StartCube.Z--;
        EndCube.Z--;
    }

    public override string ToString() 
        => $"{StartCube}~{EndCube}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var brick = (Brick) obj;
        return StartCube.Equals(brick.StartCube) && EndCube.Equals(brick.EndCube);
    }
}