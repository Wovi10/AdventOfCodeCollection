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
    private List<Brick> SafeToDelete { get; } = new();
    private List<Brick> UnsafeToDelete { get; } = new();

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
                SafeToDelete.Add(brickInLoop);

            switch (brickInLoop.BricksBelow.Count)
            {
                case 1:
                    SafeToDelete.Remove(brickInLoop.BricksBelow.First());
                    UnsafeToDelete.Add(brickInLoop.BricksBelow.First());
                    break;
                case > 1:
                    SafeToDelete.AddRange(brickInLoop.BricksBelow);
                    break;
            }
        }

        SafeToDelete.RemoveAll(brick => UnsafeToDelete.Contains(brick));

        return this;
    }

    public int CountDisintegrableBricks()
        => SafeToDelete.Distinct().Count();

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
        => UnsafeToDelete
            .Distinct()
            .Select(DisintegrateBricksAbove)
            .Sum();

    private static int DisintegrateBricksAbove(Brick brick)
    {
        var bricksToDisintegrate = new HashSet<Brick>();
        var stack = new Stack<Brick>();
        stack.Push(brick);

        while (stack.Count > 0)
        {
            var currentBrick = stack.Pop();
            foreach (var brickAbove in currentBrick.BricksAbove)
            {
                if (brickAbove.BricksBelow.Count != 1 &&
                    !brickAbove.BricksBelow.All(bricksToDisintegrate.Contains)) 
                    continue;

                if (bricksToDisintegrate.Add(brickAbove)) 
                    stack.Push(brickAbove);
            }
        }

        return bricksToDisintegrate.Count;
    }
}