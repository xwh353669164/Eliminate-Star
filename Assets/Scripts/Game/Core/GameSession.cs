using System;

public sealed class GameSession
{
    public const float DefaultMaxTime = 120f;

    public float MaxTime { get; }
    public float RemainingTime { get; private set; }
    public int Score { get; private set; }
    public GameState State { get; private set; }
    public GameEndReason EndReason { get; private set; }
    public BoardModel Board { get; private set; }

    public GameSession(float maxTime = DefaultMaxTime)
    {
        if (maxTime <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTime), "Game time must be greater than zero.");
        }

        MaxTime = maxTime;
        RemainingTime = maxTime;
        State = GameState.Ready;
        EndReason = GameEndReason.None;
    }

    public void Start(StarColor[] colors)
    {
        Board = new BoardModel(colors);
        RemainingTime = MaxTime;
        Score = 0;
        EndReason = GameEndReason.None;
        State = GameState.Playing;

        if (!Board.HasAnyLegalGroup())
        {
            End(GameEndReason.NoLegalGroup);
        }
    }

    public TurnResult Select(int row, int column)
    {
        if (State != GameState.Playing)
        {
            return TurnResult.Invalid(Board.CreateSnapshot());
        }

        TurnResult result = Board.Resolve(row, column);

        if (!result.IsValid)
        {
            return result;
        }

        Score += result.ScoreDelta;
        State = GameState.Resolving;
        return result;
    }

    public void Tick(float deltaTime)
    {
        if (State != GameState.Playing && State != GameState.Resolving)
        {
            return;
        }

        if (deltaTime <= 0f)
        {
            return;
        }

        RemainingTime = Math.Max(0f, RemainingTime - deltaTime);

        if (RemainingTime <= 0f && State == GameState.Playing)
        {
            End(GameEndReason.TimeExpired);
        }
    }

    public void CompleteResolution()
    {
        if (State != GameState.Resolving)
        {
            return;
        }

        if (RemainingTime <= 0f)
        {
            End(GameEndReason.TimeExpired);
            return;
        }

        if (!Board.HasAnyLegalGroup())
        {
            End(GameEndReason.NoLegalGroup);
            return;
        }

        State = GameState.Playing;
    }

    public void Pause()
    {
        if (State == GameState.Playing)
        {
            State = GameState.Paused;
        }
    }

    public void Resume()
    {
        if (State == GameState.Paused)
        {
            State = GameState.Playing;
        }
    }

    public void Abort()
    {
        if (State == GameState.Ready || State == GameState.Ended)
        {
            return;
        }

        End(GameEndReason.Aborted);
    }

    private void End(GameEndReason reason)
    {
        State = GameState.Ended;
        EndReason = reason;
    }
}
