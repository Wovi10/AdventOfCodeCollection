using AdventOfCode2023_1.Models.Day16;
using AdventOfCode2023_1.Models.Day16.Enums;

namespace AdventOfCode2023_Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    #region DirectionUpwards

    [Test]
    public void DirectionUpwards_EmptySpace_DirectionUpwards()
    {
        var initialDirection = Direction.Upwards;
        var actual = initialDirection.GetNewDirections(TileType.EmptySpace);
        var expected = new List<Direction> {Direction.Upwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionUpwards_VerticalSplitter_DirectionUpward()
    {
        var initialDirection = Direction.Upwards;
        var actual = initialDirection.GetNewDirections(TileType.VerticalSplitter);
        var expected = new List<Direction> {Direction.Upwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionUpwards_HorizontalSplitter_DirectionRightDirectionLeft()
    {
        var initialDirection = Direction.Upwards;
        var actual = initialDirection.GetNewDirections(TileType.HorizontalSplitter);
        var expected = new List<Direction> {Direction.Right, Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionUpwards_BottomLeftToTopRightMirror_DirectionRight()
    {
        var initialDirection = Direction.Upwards;
        var actual = initialDirection.GetNewDirections(TileType.BottomLeftToTopRightMirror);
        var expected = new List<Direction> {Direction.Right};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionUpwards_TopLeftToBottomRightMirror_DirectionLeft()
    {
        var initialDirection = Direction.Upwards;
        var actual = initialDirection.GetNewDirections(TileType.TopLeftToBottomRightMirror);
        var expected = new List<Direction> {Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    #endregion

    #region DirectionRight

    [Test]
    public void DirectionRight_EmptySpace_DirectionRight()
    {
        var initialDirection = Direction.Right;
        var actual = initialDirection.GetNewDirections(TileType.EmptySpace);
        var expected = new List<Direction> {Direction.Right};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionRight_VerticalSplitter_DirectionUpwardDirectionDownward()
    {
        var initialDirection = Direction.Right;
        var actual = initialDirection.GetNewDirections(TileType.VerticalSplitter);
        var expected = new List<Direction> {Direction.Upwards, Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionRight_HorizontalSplitter_DirectionRight()
    {
        var initialDirection = Direction.Right;
        var actual = initialDirection.GetNewDirections(TileType.HorizontalSplitter);
        var expected = new List<Direction> {Direction.Right};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionRight_BottomLeftToTopRightMirror_DirectionUpwards()
    {
        var initialDirection = Direction.Right;
        var actual = initialDirection.GetNewDirections(TileType.BottomLeftToTopRightMirror);
        var expected = new List<Direction> {Direction.Upwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionRight_TopLeftToBottomRightMirror_DirectionDownwards()
    {
        var initialDirection = Direction.Right;
        var actual = initialDirection.GetNewDirections(TileType.TopLeftToBottomRightMirror);
        var expected = new List<Direction> {Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    #endregion

    #region DirectionDownwards
    
    [Test]
    public void DirectionDownwards_EmptySpace_DirectionDownwards()
    {
        var initialDirection = Direction.Downwards;
        var actual = initialDirection.GetNewDirections(TileType.EmptySpace);
        var expected = new List<Direction> {Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionDownwards_VerticalSplitter_DirectionDownwards()
    {
        var initialDirection = Direction.Downwards;
        var actual = initialDirection.GetNewDirections(TileType.VerticalSplitter);
        var expected = new List<Direction> {Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionDownwards_HorizontalSplitter_DirectionRightDirectionLeft()
    {
        var initialDirection = Direction.Downwards;
        var actual = initialDirection.GetNewDirections(TileType.HorizontalSplitter);
        var expected = new List<Direction> {Direction.Right, Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionDownwards_BottomLeftToTopRightMirror_DirectionLeft()
    {
        var initialDirection = Direction.Downwards;
        var actual = initialDirection.GetNewDirections(TileType.BottomLeftToTopRightMirror);
        var expected = new List<Direction> {Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionDownwards_TopLeftToBottomRightMirror_DirectionRight()
    {
        var initialDirection = Direction.Downwards;
        var actual = initialDirection.GetNewDirections(TileType.TopLeftToBottomRightMirror);
        var expected = new List<Direction> {Direction.Right};
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    #endregion
    
    #region DirectionLeft

    [Test]
    public void DirectionLeft_EmptySpace_DirectionLeft()
    {
        var initialDirection = Direction.Left;
        var actual = initialDirection.GetNewDirections(TileType.EmptySpace);
        var expected = new List<Direction> {Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionLeft_VerticalSplitter_DirectionUpwardDirectionDownward()
    {
        var initialDirection = Direction.Left;
        var actual = initialDirection.GetNewDirections(TileType.VerticalSplitter);
        var expected = new List<Direction> {Direction.Upwards, Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionLeft_HorizontalSplitter_DirectionLeft()
    {
        var initialDirection = Direction.Left;
        var actual = initialDirection.GetNewDirections(TileType.HorizontalSplitter);
        var expected = new List<Direction> {Direction.Left};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionLeft_BottomLeftToTopRightMirror_DirectionDownWards()
    {
        var initialDirection = Direction.Left;
        var actual = initialDirection.GetNewDirections(TileType.BottomLeftToTopRightMirror);
        var expected = new List<Direction> {Direction.Downwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void DirectionLeft_TopLeftToBottomRightMirror_DirectionUpwards()
    {
        var initialDirection = Direction.Left;
        var actual = initialDirection.GetNewDirections(TileType.TopLeftToBottomRightMirror);
        var expected = new List<Direction> {Direction.Upwards};
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    #endregion

    #region GetTileType

    [Test]
    public void ToTileType_Dot_EmptySpace()
    {
        var cell = '.';
        var actual = cell.ToTileType();
        var expected = TileType.EmptySpace;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void ToTileType_Backslash_TopLeftToBottomRightMirror()
    {
        var cell = '\\';
        var actual = cell.ToTileType();
        var expected = TileType.TopLeftToBottomRightMirror;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void ToTileType_ForwardSlash_BottomLeftToTopRightMirror()
    {
        var cell = '/';
        var actual = cell.ToTileType();
        var expected = TileType.BottomLeftToTopRightMirror;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void ToTileType_Pipe_VerticalSplitter()
    {
        var cell = '|';
        var actual = cell.ToTileType();
        var expected = TileType.VerticalSplitter;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void ToTileType_Hyphen_HorizontalSplitter()
    {
        var cell = '-';
        var actual = cell.ToTileType();
        var expected = TileType.HorizontalSplitter;
        Assert.That(actual, Is.EqualTo(expected));
    }

    #endregion
}