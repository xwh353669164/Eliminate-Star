using System;

public class EventHandler
{
    public static event Action GameBeginEvent;
    public static void CallGameBeginEvent()
    {
        GameBeginEvent?.Invoke();
    }

    public static event Action<float, float> TimeChangeEvent;
    public static void CallTimeChangeEvent(float nowTime, float maxTime)
    {
        TimeChangeEvent?.Invoke(nowTime, maxTime);
    }

    public static event Action<bool> StopTimeEvent;
    public static void CallStopTimeEvent(bool stop)
    {
        StopTimeEvent?.Invoke(stop);
    }

    public static event Action<BoardSnapshot> BoardCreateEvent;
    public static void CallBoardCreateEvent(BoardSnapshot board)
    {
        BoardCreateEvent?.Invoke(board);
    }

    public static event Action<int, int> StarClickEvent;
    public static void CallStarClickEvent(int row, int column)
    {
        StarClickEvent?.Invoke(row, column);
    }

    public static event Action<int, int> InvalidClickEvent;
    public static void CallInvalidClickEvent(int row, int column)
    {
        InvalidClickEvent?.Invoke(row, column);
    }

    public static event Action<TurnResult> BoardResolveEvent;
    public static void CallBoardResolveEvent(TurnResult result)
    {
        BoardResolveEvent?.Invoke(result);
    }

    public static event Action BoardResolveCompleteEvent;
    public static void CallBoardResolveCompleteEvent()
    {
        BoardResolveCompleteEvent?.Invoke();
    }

    public static event Action<int> ScoreChangeEvent;
    public static void CallScoreChangeEvent(int score)
    {
        ScoreChangeEvent?.Invoke(score);
    }

    public static event Action<GameState> GameStateChangeEvent;
    public static void CallGameStateChangeEvent(GameState state)
    {
        GameStateChangeEvent?.Invoke(state);
    }

    public static event Action<int, GameEndReason> GameEndEvent;
    public static void CallGameEndEvent(int score, GameEndReason reason)
    {
        GameEndEvent?.Invoke(score, reason);
    }

    public static event Action GameAbortEvent;
    public static void CallGameAbortEvent()
    {
        GameAbortEvent?.Invoke();
    }
}
