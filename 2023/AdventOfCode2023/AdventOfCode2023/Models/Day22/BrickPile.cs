namespace AdventOfCode2023_1.Models.Day22;

public class BrickPile
{
    public BrickPile(List<string> input)
    {
        Bricks = new List<Brick>();
        foreach (var inputLine in input.Where(line => !string.IsNullOrWhiteSpace(line)))
        {
            var brickToAdd = new Brick(inputLine);

            if (brickToAdd.IsValid)
                Bricks.Add(brickToAdd);
        }
    }

    public List<Brick> Bricks { get; private set; }

    public BrickPile OrderBricks()
    {
        Bricks = Bricks.OrderBy(b => b.Lowest).ToList();
        return this;
    }

    public bool CanMoveDown(Brick brick)
        => !brick.IsAtBottom && !HasBrickBelow(brick);

    private bool HasBrickBelow(Brick inputBrick)
        => Bricks
            .Any(otherBrick => (otherBrick.Highest == inputBrick.Lowest - 1) && otherBrick.IntersectsOnX_Y(inputBrick));

    public int CountDisintegrableBricks()
    {
        var bricksToDisintegrate = new List<Brick>();
        var okayBricks = new List<Brick>();
        foreach (var brickInLoop in Bricks)
        {
            SetBricksAbove(brickInLoop);
            SetBrickBelow(brickInLoop);

            if (!brickInLoop.BricksAbove.Any())
                bricksToDisintegrate.Add(brickInLoop);

            switch (brickInLoop.BricksBelow.Count)
            {
                case 1:
                    bricksToDisintegrate.Remove(brickInLoop.BricksBelow.First());
                    okayBricks.Add(brickInLoop.BricksBelow.First());
                    break;
                case > 1:
                    bricksToDisintegrate.AddRange(brickInLoop.BricksBelow);
                    break;
            }
        }

        bricksToDisintegrate.RemoveAll(brick => okayBricks.Contains(brick));

        return bricksToDisintegrate.Distinct().Count();
    }

    private void SetBrickBelow(Brick brickInLoop)
        => brickInLoop.BricksBelow = Bricks
            .Where(otherBrick =>
                otherBrick.Highest == brickInLoop.Lowest - 1 &&
                otherBrick.IntersectsOnX_Y(brickInLoop))
            .ToList();

    private void SetBricksAbove(Brick brick)
        => brick.BricksAbove = Bricks
            .Where(otherBrick =>
                otherBrick.Lowest == brick.Highest + 1 &&
                otherBrick.IntersectsOnX_Y(brick))
            .ToList();
}