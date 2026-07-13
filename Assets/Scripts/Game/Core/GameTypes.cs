using System;

public enum StarColor
{
    Blue,
    Green,
    Purple,
    Red,
    Yellow,
    Empty
}

public enum GameState
{
    Ready,
    Playing,
    Resolving,
    Paused,
    Ended
}

public enum GameEndReason
{
    None,
    TimeExpired,
    NoLegalGroup,
    Aborted
}

public struct StarData
{
    public int Id { get; }
    public StarColor Color { get; }
    public int Row { get; }
    public int Column { get; }
    public bool IsEmpty => Color == StarColor.Empty;

    public StarData(int id, StarColor color, int row, int column)
    {
        Id = id;
        Color = color;
        Row = row;
        Column = column;
    }

    public StarData MoveTo(int row, int column)
    {
        return new StarData(Id, Color, row, column);
    }

    public static StarData CreateEmpty(int row, int column)
    {
        return new StarData(-1, StarColor.Empty, row, column);
    }
}

public struct StarMove
{
    public int StarId { get; }
    public int FromRow { get; }
    public int FromColumn { get; }
    public int ToRow { get; }
    public int ToColumn { get; }

    public StarMove(int starId, int fromRow, int fromColumn, int toRow, int toColumn)
    {
        StarId = starId;
        FromRow = fromRow;
        FromColumn = fromColumn;
        ToRow = toRow;
        ToColumn = toColumn;
    }
}

public sealed class BoardSnapshot
{
    public int Rows { get; }
    public int Columns { get; }
    public StarData[] Stars { get; }

    public BoardSnapshot(int rows, int columns, StarData[] stars)
    {
        Rows = rows;
        Columns = columns;
        Stars = stars;
    }
}

public sealed class TurnResult
{
    private static readonly StarData[] EmptyStars = Array.Empty<StarData>();
    private static readonly StarMove[] EmptyMoves = Array.Empty<StarMove>();

    public bool IsValid { get; }
    public StarData[] RemovedStars { get; }
    public StarMove[] VerticalMoves { get; }
    public StarMove[] HorizontalMoves { get; }
    public int ScoreDelta { get; }
    public BoardSnapshot Board { get; }

    private TurnResult(
        bool isValid,
        StarData[] removedStars,
        StarMove[] verticalMoves,
        StarMove[] horizontalMoves,
        int scoreDelta,
        BoardSnapshot board)
    {
        IsValid = isValid;
        RemovedStars = removedStars;
        VerticalMoves = verticalMoves;
        HorizontalMoves = horizontalMoves;
        ScoreDelta = scoreDelta;
        Board = board;
    }

    public static TurnResult Invalid(BoardSnapshot board)
    {
        return new TurnResult(false, EmptyStars, EmptyMoves, EmptyMoves, 0, board);
    }

    public static TurnResult Valid(
        StarData[] removedStars,
        StarMove[] verticalMoves,
        StarMove[] horizontalMoves,
        BoardSnapshot board)
    {
        int count = removedStars.Length;
        int scoreDelta = 5 * count * count;
        return new TurnResult(true, removedStars, verticalMoves, horizontalMoves, scoreDelta, board);
    }
}
