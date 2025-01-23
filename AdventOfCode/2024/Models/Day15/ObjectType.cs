namespace _2024.Models.Day15;

public enum ObjectType
{
    Wall,
    Empty,
    Box,
    Robot
}

public enum ObjectType2
{
    Wall,
    Empty,
    BoxLeft,
    BoxRight,
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

    public static char ToChar(this ObjectType2 objectType)
    {
        return objectType switch
        {
            ObjectType2.Wall => '#',
            ObjectType2.Empty => '.',
            ObjectType2.BoxLeft => '[',
            ObjectType2.BoxRight => ']',
            ObjectType2.Robot => '@',
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    }

    public static ObjectType2 ToObjectType2(this char c)
    {
        return c switch
        {
            '#' => ObjectType2.Wall,
            '.' => ObjectType2.Empty,
            '[' => ObjectType2.BoxLeft,
            ']' => ObjectType2.BoxRight,
            '@' => ObjectType2.Robot,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}