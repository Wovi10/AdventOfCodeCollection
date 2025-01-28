namespace _2024.Models.Day16;

public enum ObjectType
{
    Wall,
    Start,
    End,
    Empty
}

public static class ObjectTypeExtensions
{
    public static char ToChar(this ObjectType objectType)
    {
        return objectType switch
        {
            ObjectType.Wall => '#',
            ObjectType.Start => 'S',
            ObjectType.End => 'E',
            ObjectType.Empty => '.',
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    }

    public static ObjectType FromChar(this char c)
    {
        return c switch
        {
            '#' => ObjectType.Wall,
            'S' => ObjectType.Start,
            'E' => ObjectType.End,
            '.' => ObjectType.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}