using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private enum AnimationPhase
    {
        Idle,
        Removing,
        VerticalFall,
        HorizontalShift
    }

    private struct MoveAnimation
    {
        public StarView View { get; }
        public Vector2 StartPosition { get; }
        public Vector2 TargetPosition { get; }
        public int TargetRow { get; }
        public int TargetColumn { get; }

        public MoveAnimation(
            StarView view,
            Vector2 startPosition,
            Vector2 targetPosition,
            int targetRow,
            int targetColumn)
        {
            View = view;
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            TargetRow = targetRow;
            TargetColumn = targetColumn;
        }
    }

    public RectTransform board;
    [Min(0.01f)] public float removeDuration = 0.12f;
    [Min(0.01f)] public float verticalFallDuration = 0.18f;
    [Min(0.01f)] public float horizontalShiftDuration = 0.18f;
    [Min(0.01f)] public float invalidFeedbackDuration = 0.18f;

    private readonly Dictionary<int, StarView> starViews = new Dictionary<int, StarView>();
    private readonly List<StarView> removingViews = new List<StarView>();
    private readonly List<MoveAnimation> moveAnimations = new List<MoveAnimation>();

    private CanvasGroup boardCanvasGroup;
    private TurnResult resolvingResult;
    private AnimationPhase animationPhase;
    private float phaseTime;
    private int rows = 10;
    private int columns = 10;
    private bool inputEnabled;

    private void Awake()
    {
        boardCanvasGroup = gameObject.AddComponent<CanvasGroup>();
        SetInputEnabled(false);
    }

    private void OnEnable()
    {
        EventHandler.BoardCreateEvent += OnBoardCreateEvent;
        EventHandler.BoardResolveEvent += OnBoardResolveEvent;
        EventHandler.InvalidClickEvent += OnInvalidClickEvent;
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.BoardCreateEvent -= OnBoardCreateEvent;
        EventHandler.BoardResolveEvent -= OnBoardResolveEvent;
        EventHandler.InvalidClickEvent -= OnInvalidClickEvent;
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
    }

    private void Update()
    {
        switch (animationPhase)
        {
            case AnimationPhase.Removing:
                UpdateRemoving();
                return;
            case AnimationPhase.VerticalFall:
                UpdateMoving(verticalFallDuration);
                return;
            case AnimationPhase.HorizontalShift:
                UpdateMoving(horizontalShiftDuration);
                return;
        }
    }

    private void OnBoardCreateEvent(BoardSnapshot snapshot)
    {
        ClearBoard();
        rows = snapshot.Rows;
        columns = snapshot.Columns;

        for (int i = 0; i < snapshot.Stars.Length; i++)
            CreateStar(snapshot.Stars[i]);
    }

    private void OnBoardResolveEvent(TurnResult result)
    {
        resolvingResult = result;
        SetInputEnabled(false);
        removingViews.Clear();

        for (int i = 0; i < result.RemovedStars.Length; i++)
            removingViews.Add(starViews[result.RemovedStars[i].Id]);

        phaseTime = 0f;
        animationPhase = AnimationPhase.Removing;
    }

    private void OnInvalidClickEvent(int row, int column)
    {
        foreach (StarView starView in starViews.Values)
        {
            if (starView.Row != row || starView.Column != column)
                continue;

            starView.PlayInvalidFeedback();
            return;
        }
    }

    private void OnGameStateChangeEvent(GameState state)
    {
        SetInputEnabled(state == GameState.Playing);
    }

    private void CreateStar(StarData data)
    {
        int prefabIndex = (int)data.Color + 1;
        string prefabPath = Path.Combine("Prefabs", "Star" + prefabIndex);
        GameObject starObject = ResManager.Instance.Load<GameObject>(prefabPath, board);
        RectTransform starTransform = (RectTransform)starObject.transform;
        starTransform.anchoredPosition = GetCellPosition(data.Row, data.Column);
        starTransform.sizeDelta = GetCellSize();
        starTransform.localScale = Vector3.one;

        StarView starView = starObject.AddComponent<StarView>();
        starView.Bind(data, invalidFeedbackDuration);
        starView.SetInputEnabled(inputEnabled);
        starViews.Add(data.Id, starView);
    }

    private void UpdateRemoving()
    {
        float progress = GetPhaseProgress(removeDuration);
        float scale = 1f - progress;

        for (int i = 0; i < removingViews.Count; i++)
            removingViews[i].SetScale(scale);

        if (progress < 1f)
            return;

        for (int i = 0; i < resolvingResult.RemovedStars.Length; i++)
        {
            int starId = resolvingResult.RemovedStars[i].Id;
            Destroy(starViews[starId].gameObject);
            starViews.Remove(starId);
        }

        BeginVerticalFall();
    }

    private void BeginVerticalFall()
    {
        BuildMoveAnimations(resolvingResult.VerticalMoves);

        if (moveAnimations.Count == 0)
        {
            BeginHorizontalShift();
            return;
        }

        phaseTime = 0f;
        animationPhase = AnimationPhase.VerticalFall;
    }

    private void BeginHorizontalShift()
    {
        BuildMoveAnimations(resolvingResult.HorizontalMoves);

        if (moveAnimations.Count == 0)
        {
            CompleteResolution();
            return;
        }

        phaseTime = 0f;
        animationPhase = AnimationPhase.HorizontalShift;
    }

    private void BuildMoveAnimations(StarMove[] moves)
    {
        moveAnimations.Clear();

        for (int i = 0; i < moves.Length; i++)
        {
            StarMove move = moves[i];
            StarView starView = starViews[move.StarId];
            MoveAnimation animation = new MoveAnimation(
                starView,
                starView.RectTransform.anchoredPosition,
                GetCellPosition(move.ToRow, move.ToColumn),
                move.ToRow,
                move.ToColumn);
            moveAnimations.Add(animation);
        }
    }

    private void UpdateMoving(float duration)
    {
        float progress = GetPhaseProgress(duration);

        for (int i = 0; i < moveAnimations.Count; i++)
        {
            MoveAnimation animation = moveAnimations[i];
            Vector2 position = Vector2.Lerp(
                animation.StartPosition,
                animation.TargetPosition,
                progress);
            animation.View.SetPosition(position);
        }

        if (progress < 1f)
            return;

        for (int i = 0; i < moveAnimations.Count; i++)
        {
            MoveAnimation animation = moveAnimations[i];
            animation.View.SetGridPosition(animation.TargetRow, animation.TargetColumn);
        }

        if (animationPhase == AnimationPhase.VerticalFall)
        {
            BeginHorizontalShift();
            return;
        }

        CompleteResolution();
    }

    private float GetPhaseProgress(float duration)
    {
        phaseTime += Time.deltaTime;
        return Mathf.Clamp01(phaseTime / duration);
    }

    private void CompleteResolution()
    {
        animationPhase = AnimationPhase.Idle;
        moveAnimations.Clear();
        removingViews.Clear();
        EventHandler.CallBoardResolveCompleteEvent();
    }

    private void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        boardCanvasGroup.interactable = enabled;
        boardCanvasGroup.blocksRaycasts = enabled;

        foreach (StarView starView in starViews.Values)
            starView.SetInputEnabled(enabled);
    }

    private Vector2 GetCellPosition(int row, int column)
    {
        Vector2 boardSize = board.rect.size;
        Vector2 cellSize = GetCellSize();
        float startX = -boardSize.x * 0.5f + cellSize.x * 0.5f;
        float startY = boardSize.y * 0.5f - cellSize.y * 0.5f;
        return new Vector2(startX + column * cellSize.x, startY - row * cellSize.y);
    }

    private Vector2 GetCellSize()
    {
        Vector2 boardSize = board.rect.size;
        return new Vector2(boardSize.x / columns, boardSize.y / rows);
    }

    private void ClearBoard()
    {
        foreach (StarView starView in starViews.Values)
            Destroy(starView.gameObject);

        starViews.Clear();
        removingViews.Clear();
        moveAnimations.Clear();
        animationPhase = AnimationPhase.Idle;
    }
}
