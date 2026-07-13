using System;
using System.Collections.Generic;

public sealed class BoardModel
{
    public const int RowCount = 10;
    public const int ColumnCount = 10;
    public const int CellCount = RowCount * ColumnCount;

    private static readonly int[] RowDirections = { -1, 1, 0, 0 };
    private static readonly int[] ColumnDirections = { 0, 0, -1, 1 };

    private readonly StarData[,] grid = new StarData[RowCount, ColumnCount];

    public int Rows => RowCount;
    public int Columns => ColumnCount;

    public BoardModel(StarColor[] colors)
    {
        if (colors.Length != CellCount)
        {
            throw new ArgumentException($"The board requires exactly {CellCount} colors.", nameof(colors));
        }

        for (int row = 0; row < RowCount; row++)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                int id = row * ColumnCount + column;
                StarColor color = colors[id];

                if ((int)color < (int)StarColor.Blue || color > StarColor.Yellow)
                {
                    throw new ArgumentOutOfRangeException(nameof(colors), "Initial colors must be one of the five playable colors.");
                }

                grid[row, column] = new StarData(id, color, row, column);
            }
        }
    }

    public StarData GetStarAt(int row, int column)
    {
        if (!IsInside(row, column))
        {
            throw new ArgumentOutOfRangeException(nameof(row), "The requested cell is outside the board.");
        }

        return grid[row, column];
    }

    public TurnResult Resolve(int row, int column)
    {
        if (!IsInside(row, column))
        {
            return TurnResult.Invalid(CreateSnapshot());
        }

        StarData selected = grid[row, column];

        if (selected.IsEmpty)
        {
            return TurnResult.Invalid(CreateSnapshot());
        }

        List<StarData> group = FindConnectedGroup(row, column);

        if (group.Count < 2)
        {
            return TurnResult.Invalid(CreateSnapshot());
        }

        Remove(group);
        StarMove[] verticalMoves = CompressVertically();
        StarMove[] horizontalMoves = ShiftColumnsLeft();

        return TurnResult.Valid(
            group.ToArray(),
            verticalMoves,
            horizontalMoves,
            CreateSnapshot());
    }

    public bool HasAnyLegalGroup()
    {
        for (int row = 0; row < RowCount; row++)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                StarData star = grid[row, column];

                if (star.IsEmpty)
                {
                    continue;
                }

                if (column + 1 < ColumnCount && grid[row, column + 1].Color == star.Color)
                {
                    return true;
                }

                if (row + 1 < RowCount && grid[row + 1, column].Color == star.Color)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public BoardSnapshot CreateSnapshot()
    {
        List<StarData> stars = new List<StarData>(CellCount);

        for (int row = 0; row < RowCount; row++)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                StarData star = grid[row, column];

                if (!star.IsEmpty)
                {
                    stars.Add(star);
                }
            }
        }

        return new BoardSnapshot(RowCount, ColumnCount, stars.ToArray());
    }

    private List<StarData> FindConnectedGroup(int startRow, int startColumn)
    {
        StarColor color = grid[startRow, startColumn].Color;
        bool[,] visited = new bool[RowCount, ColumnCount];
        Queue<int> pending = new Queue<int>();
        List<StarData> group = new List<StarData>();

        pending.Enqueue(startRow * ColumnCount + startColumn);
        visited[startRow, startColumn] = true;

        while (pending.Count > 0)
        {
            int index = pending.Dequeue();
            int row = index / ColumnCount;
            int column = index % ColumnCount;
            group.Add(grid[row, column]);

            for (int direction = 0; direction < RowDirections.Length; direction++)
            {
                int nextRow = row + RowDirections[direction];
                int nextColumn = column + ColumnDirections[direction];

                if (!IsInside(nextRow, nextColumn) || visited[nextRow, nextColumn])
                {
                    continue;
                }

                if (grid[nextRow, nextColumn].Color != color)
                {
                    continue;
                }

                visited[nextRow, nextColumn] = true;
                pending.Enqueue(nextRow * ColumnCount + nextColumn);
            }
        }

        return group;
    }

    private void Remove(List<StarData> stars)
    {
        for (int index = 0; index < stars.Count; index++)
        {
            StarData star = stars[index];
            grid[star.Row, star.Column] = StarData.CreateEmpty(star.Row, star.Column);
        }
    }

    private StarMove[] CompressVertically()
    {
        List<StarMove> moves = new List<StarMove>();

        for (int column = 0; column < ColumnCount; column++)
        {
            int writeRow = RowCount - 1;

            for (int readRow = RowCount - 1; readRow >= 0; readRow--)
            {
                StarData star = grid[readRow, column];

                if (star.IsEmpty)
                {
                    continue;
                }

                if (readRow != writeRow)
                {
                    grid[writeRow, column] = star.MoveTo(writeRow, column);
                    grid[readRow, column] = StarData.CreateEmpty(readRow, column);
                    moves.Add(new StarMove(star.Id, readRow, column, writeRow, column));
                }

                writeRow--;
            }

            for (int row = writeRow; row >= 0; row--)
            {
                grid[row, column] = StarData.CreateEmpty(row, column);
            }
        }

        return moves.ToArray();
    }

    private StarMove[] ShiftColumnsLeft()
    {
        List<StarMove> moves = new List<StarMove>();
        int writeColumn = 0;

        for (int readColumn = 0; readColumn < ColumnCount; readColumn++)
        {
            if (IsColumnEmpty(readColumn))
            {
                continue;
            }

            if (readColumn != writeColumn)
            {
                MoveColumn(readColumn, writeColumn, moves);
            }

            writeColumn++;
        }

        for (int column = writeColumn; column < ColumnCount; column++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                grid[row, column] = StarData.CreateEmpty(row, column);
            }
        }

        return moves.ToArray();
    }

    private void MoveColumn(int fromColumn, int toColumn, List<StarMove> moves)
    {
        for (int row = 0; row < RowCount; row++)
        {
            StarData star = grid[row, fromColumn];

            if (star.IsEmpty)
            {
                continue;
            }

            grid[row, toColumn] = star.MoveTo(row, toColumn);
            grid[row, fromColumn] = StarData.CreateEmpty(row, fromColumn);
            moves.Add(new StarMove(star.Id, row, fromColumn, row, toColumn));
        }
    }

    private bool IsColumnEmpty(int column)
    {
        return grid[RowCount - 1, column].IsEmpty;
    }

    private static bool IsInside(int row, int column)
    {
        return row >= 0 && row < RowCount && column >= 0 && column < ColumnCount;
    }
}
