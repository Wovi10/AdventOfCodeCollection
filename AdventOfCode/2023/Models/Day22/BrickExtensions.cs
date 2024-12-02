namespace _2023.Models.Day22;

public static class BrickExtensions
{
    public static BrickPile CreateBrickPile(this List<string> input) 
        => new(input);
    
    public static BrickPile MoveBricksDown(this BrickPile brickPile)
    {
        brickPile.Bricks
            .ForEach(brick =>
            {
                while (brickPile.CanMoveDown(brick)) 
                    brick.MoveDown();
            });
        return brickPile;
    }
}