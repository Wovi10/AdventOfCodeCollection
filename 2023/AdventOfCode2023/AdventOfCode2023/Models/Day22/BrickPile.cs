namespace AdventOfCode2023_1.Models.Day22;

public class BrickPile
{
    public BrickPile(List<string> input)
    {
        Bricks = new List<Brick>();

        foreach (var newBrick
                 in input
                     .Where(line => !string.IsNullOrWhiteSpace(line))
                     .Select(line => new Brick(line.Trim())))
        {
            if (newBrick.IsValid)
                Bricks.Add(newBrick);
        }

        Bricks = GetOrderedBricks();
    }

    public List<Brick> Bricks { get; set; }

    private List<Brick> GetOrderedBricks()
        => Bricks.OrderBy(b => b.LowestZ).ThenBy(b => b.LowestX).ThenBy(b => b.LowestY).ToList();

    public void MoveBricksDown()
    {
        foreach (var brick in Bricks)
            while (CanMoveDown(brick))
                brick.MoveDown();
    }

    private bool CanMoveDown(Brick brick) 
        => brick.CanMoveDown && !HasBrickBelow(brick);

    private bool HasBrickBelow(Brick brick) 
        => LevelBelowIsUsed(brick) && HasBrickTouching(brick);

    private bool LevelBelowIsUsed(Brick brick)
        => Bricks
            .Where(b => !Equals(b, brick))
            .Any(b => b.Cubes.Any(c => c.Z == brick.LowestZ - 1));

    private bool HasBrickTouching(Brick brick) 
        => Bricks
            .Where(b => b.Cubes.Any(c => c.Z == brick.LowestZ - 1))
            .ToList()
            .Any(b => b.Cubes.Any(c => brick.Cubes.Any(bc => bc.X == c.X && bc.Y == c.Y)));

    public void Print()
    {
        foreach (var brick in Bricks)
            Console.WriteLine(brick);
    }

    public int CountDisintegrableBricks()
    {
        var bricksToDisintegrate = new List<Brick>();

        Bricks.Reverse();

        foreach (var brick in Bricks)
        {
            if (!IsSupportingBricks(brick)) 
                bricksToDisintegrate.Add(brick);

            var supportingBricks = Bricks
                .Where(b => b.Cubes.Any(c => c.Z == brick.LowestZ - 1))
                .Where(b => b.Cubes.Any(c => brick.Cubes.Any(bc => bc.X == c.X && bc.Y == c.Y)))
                .ToList();

            if (supportingBricks.Count == 1 && bricksToDisintegrate.Contains(supportingBricks.First())) 
                bricksToDisintegrate.Remove(supportingBricks.First());

            if (supportingBricks.Count <= 1)
                continue;

            foreach (var touchingBrick in supportingBricks.Where(touchingBrick => !bricksToDisintegrate.Contains(touchingBrick)))
                bricksToDisintegrate.Add(touchingBrick);
        }

        return bricksToDisintegrate.Count;
    }

    private bool IsSupportingBricks(Brick brick) 
        => Bricks
            .Where(b => !Equals(b, brick))
            .Where(b => b.Cubes.Any(c => c.Z == brick.HighestZ + 1))
            .Any(b => b.Cubes.Any(c => brick.Cubes.Any(bc => bc.X == c.X && bc.Y == c.Y)));
}