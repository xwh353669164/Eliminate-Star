using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxTime = GameSession.DefaultMaxTime;

    private GameSession session;
    private bool endPublished;

    private void Awake()
    {
        session = new GameSession(maxTime);
    }

    private void OnEnable()
    {
        EventHandler.GameBeginEvent += OnGameBeginEvent;
        EventHandler.StopTimeEvent += OnStopTimeEvent;
        EventHandler.StarClickEvent += OnStarClickEvent;
        EventHandler.BoardResolveCompleteEvent += OnBoardResolveCompleteEvent;
        EventHandler.GameAbortEvent += OnGameAbortEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameBeginEvent -= OnGameBeginEvent;
        EventHandler.StopTimeEvent -= OnStopTimeEvent;
        EventHandler.StarClickEvent -= OnStarClickEvent;
        EventHandler.BoardResolveCompleteEvent -= OnBoardResolveCompleteEvent;
        EventHandler.GameAbortEvent -= OnGameAbortEvent;
    }

    private void Update()
    {
        GameState previousState = session.State;
        float previousTime = session.RemainingTime;

        session.Tick(Time.deltaTime);

        if (session.RemainingTime != previousTime)
            EventHandler.CallTimeChangeEvent(session.RemainingTime, session.MaxTime);

        PublishTransition(previousState);
    }

    private void OnGameBeginEvent()
    {
        session = new GameSession(maxTime);
        endPublished = false;
        session.Start(CreateRandomColors());

        EventHandler.CallBoardCreateEvent(session.Board.CreateSnapshot());
        EventHandler.CallScoreChangeEvent(session.Score);
        EventHandler.CallTimeChangeEvent(session.RemainingTime, session.MaxTime);
        EventHandler.CallGameStateChangeEvent(session.State);

        if (session.State == GameState.Ended)
            PublishEnd();
    }

    private void OnStarClickEvent(int row, int column)
    {
        if (session.State != GameState.Playing)
            return;

        TurnResult result = session.Select(row, column);
        if (!result.IsValid)
        {
            EventHandler.CallInvalidClickEvent(row, column);
            return;
        }

        EventHandler.CallScoreChangeEvent(session.Score);
        EventHandler.CallGameStateChangeEvent(session.State);
        EventHandler.CallBoardResolveEvent(result);
    }

    private void OnBoardResolveCompleteEvent()
    {
        if (session.State != GameState.Resolving)
            return;

        GameState previousState = session.State;
        session.CompleteResolution();
        PublishTransition(previousState);
    }

    private void OnStopTimeEvent(bool stop)
    {
        GameState previousState = session.State;
        if (stop)
        {
            session.Pause();
            PublishTransition(previousState);
            return;
        }

        session.Resume();
        PublishTransition(previousState);
    }

    private void OnGameAbortEvent()
    {
        GameState previousState = session.State;
        session.Abort();
        PublishTransition(previousState);
    }

    private void PublishTransition(GameState previousState)
    {
        if (previousState == session.State)
            return;

        EventHandler.CallGameStateChangeEvent(session.State);
        if (session.State == GameState.Ended)
            PublishEnd();
    }

    private void PublishEnd()
    {
        if (endPublished)
            return;

        endPublished = true;
        if (session.EndReason != GameEndReason.Aborted)
            LocalDataService.Instance.AddRank(session.Score, DateTime.Now);

        EventHandler.CallGameEndEvent(session.Score, session.EndReason);
    }

    private static StarColor[] CreateRandomColors()
    {
        StarColor[] colors = new StarColor[BoardModel.CellCount];
        for (int index = 0; index < colors.Length; index++)
            colors[index] = (StarColor)UnityEngine.Random.Range(0, 5);

        return colors;
    }
}
