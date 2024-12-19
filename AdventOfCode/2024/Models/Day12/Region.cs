namespace _2024.Models.Day12;

public class Region
{
    public char PlantType { get; init; }
    public List<Coordinate> Coordinates { get; set; } = new();
    public int PerimeterCount => Coordinates
                                    .Select(coordinate => coordinate.GetNeighbours())
                                    .Select(neighbours => neighbours.Count(n => !Coordinates.Contains(n)))
                                    .Sum();

    public Region(char plantType, Coordinate firstCoordinate)
    {
        PlantType = plantType;
        Coordinates.Add(firstCoordinate);
    }

    public void AddToCoordinates(Coordinate coordinate)
    {
        if (!Coordinates.Contains(coordinate))
            Coordinates.Add(coordinate);
    }
}