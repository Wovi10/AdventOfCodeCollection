namespace _2024.Models.Day15;

public enum ObjectType
{
    Wall,
    Empty,
    Box,
    Robot
}

public static class ObjectTypeExtensions
{
    public static char ToChar(this ObjectType objectType)
    {
        return objectType switch
        {
            ObjectType.Wall => '#',
            ObjectType.Empty => '.',
            ObjectType.Box => 'O',
            ObjectType.Robot => '@',
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    }

    public static ObjectType FromChar(this char c)
    {
        return c switch
        {
            '#' => ObjectType.Wall,
            '.' => ObjectType.Empty,
            'O' => ObjectType.Box,
            '@' => ObjectType.Robot,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}