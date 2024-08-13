using UtilsCSharp;

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
    public List<Brick> DisintegrableBricks { get; } = new();
    private List<Brick> NonDisintegrableBricks { get; } = new();

    public BrickPile OrderBricks(bool ascending = true)
    {
        Bricks =
            ascending
                ? Bricks.OrderBy(b => b.Lowest).ToList()
                : Bricks.OrderByDescending(b => b.Lowest).ToList();

        return this;
    }

    public bool CanMoveDown(Brick brick)
        => !brick.IsAtBottom && !HasBrickBelow(brick);

    private bool HasBrickBelow(Brick inputBrick)
        => Bricks
            .Any(otherBrick => (otherBrick.Highest == inputBrick.Lowest - 1) && otherBrick.IntersectsOnX_Y(inputBrick));

    public BrickPile FindDisintegrableBricks()
    {
        foreach (var brickInLoop in Bricks)
        {
            SetBricksAbove(brickInLoop);
            SetBrickBelow(brickInLoop);

            if (!brickInLoop.BricksAbove.Any())
                DisintegrableBricks.Add(brickInLoop);

            switch (brickInLoop.BricksBelow.Count)
            {
                case 1:
                    DisintegrableBricks.Remove(brickInLoop.BricksBelow.First());
                    NonDisintegrableBricks.Add(brickInLoop.BricksBelow.First());
                    break;
                case > 1:
                    DisintegrableBricks.AddRange(brickInLoop.BricksBelow);
                    break;
            }
        }

        DisintegrableBricks.RemoveAll(brick => NonDisintegrableBricks.Contains(brick));

        return this;
    }

    public int CountDisintegrableBricks()
        => DisintegrableBricks.Distinct().Count();

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

    public int CountChainReaction()
    {
        var highestCount = 0;
        var counts = new List<int>();
        foreach (var brick in Bricks)
        {
            var count = brick.GetDisintegratedBricksCount();
            counts.Add(count);
            highestCount = Comparisons.GetHighest(count, highestCount);
        }

        return highestCount;
    }
}