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

        OrderBricks();
    }

    public List<Brick> Bricks { get; set; }


    public void OrderBricks()
        => Bricks = GetOrderedBricks();

    private List<Brick> GetOrderedBricks()
        => Bricks.OrderBy(b => b.HighestZ).ToList();

    public void MoveBricksDown()
    {
        var visitedBricks = new HashSet<Brick>();

        foreach (var brick in Bricks)
        {
            if (visitedBricks.Contains(brick))
                continue;

            while (CanMoveDown(brick))
            {
                brick.MoveDown();
                visitedBricks.Add(brick);
            }
        }
    }

    private bool CanMoveDown(Brick brick) 
        => !brick.IsAtBottom && !HasBrickBelow(brick);

    private bool HasBrickBelow(Brick inputBrick) 
        => Bricks
            .Any(b => b.HighestZ == inputBrick.LowestZ - 1 && b.IntersectsXY(inputBrick));

    public int CountDisintegrableBricks()
    {
        var bricksToDisintegrate = new HashSet<Brick>();

        var okayBricks = new HashSet<Brick>();
        foreach (var inputBrick in Bricks)
        {
            inputBrick.BricksBelow = GetBrickBelow(inputBrick);
            inputBrick.BricksAbove = GetBricksAbove(inputBrick);

            if (!inputBrick.IsSupporting())
                bricksToDisintegrate.Add(inputBrick);

            switch (inputBrick.BricksBelow.Count)
            {
                case 0 when inputBrick.LowestZ != 1:
                    throw new Exception($"Brick has no bricks below: {inputBrick}");

                case 1 when bricksToDisintegrate.Contains(inputBrick.BricksBelow.First()):
                    bricksToDisintegrate.Remove(inputBrick.BricksBelow.First());
                    okayBricks.Add(inputBrick.BricksBelow.First());
                    break;

                case > 1:
                    bricksToDisintegrate.UnionWith(inputBrick.BricksBelow);
                    break;
            }
        }

        foreach (var brickOkay in okayBricks)
            bricksToDisintegrate.Remove(brickOkay);

        return bricksToDisintegrate.Count;
    }

    private List<Brick> GetBrickBelow(Brick brick) 
        => Bricks
            .Where(otherBrick =>
                otherBrick.HighestZ == brick.LowestZ - 1 &&
                otherBrick.IntersectsXY(brick))
            .ToList();

    private List<Brick> GetBricksAbove(Brick brick)
        => Bricks
            .Where(otherBrick =>
                otherBrick.LowestZ == brick.HighestZ + 1 &&
                otherBrick.IntersectsXY(brick))
            .ToList();

    public void Print()
    {
        foreach (var brick in Bricks)
            Console.WriteLine(brick);
    }
}