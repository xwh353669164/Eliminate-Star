using UnityEngine.UI;

public class EndPanel : BasePanel
{
    public Button btnAgain;
    public Button btnReturn;
    public Text score;

    public override void Init()
    {
        btnAgain.onClick.AddListener(BeginAgain);
        btnReturn.onClick.AddListener(ReturnToBegin);
    }

    public override void UnInit()
    {
        btnAgain.onClick.RemoveAllListeners();
        btnReturn.onClick.RemoveAllListeners();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
    }

    protected override void OnDisable()
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        base.OnDisable();
    }

    public void SetScore(int value)
    {
        score.text = value.ToString();
        ShowMe();
    }

    private void BeginAgain()
    {
        EventHandler.CallGameBeginEvent();
    }

    private void OnGameStateChangeEvent(GameState state)
    {
        if (state != GameState.Playing)
            return;

        UIManager.Instance.HidePanel<EndPanel>();
    }

    private void ReturnToBegin()
    {
        UIManager.Instance.HidePanel<EndPanel>();
        UIManager.Instance.HidePanel<GamePanel>();
        UIManager.Instance.ShowPanel<BeginPanel>();
    }
}
