using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button btnReturn;
    public Button btnSetting;
    public Slider sliderTime;
    public Text score;

    public override void Init()
    {
        score.text = "0";
        sliderTime.value = 1f;
        SetMenuButtonsEnabled(false);
        btnReturn.onClick.AddListener(OpenReturnPanel);
        btnSetting.onClick.AddListener(OpenSettingPanel);
    }

    public override void UnInit()
    {
        btnReturn.onClick.RemoveAllListeners();
        btnSetting.onClick.RemoveAllListeners();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.TimeChangeEvent += OnTimeChangeEvent;
        EventHandler.ScoreChangeEvent += OnScoreChangeEvent;
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.GameEndEvent += OnGameEndEvent;
    }

    protected override void OnDisable()
    {
        EventHandler.TimeChangeEvent -= OnTimeChangeEvent;
        EventHandler.ScoreChangeEvent -= OnScoreChangeEvent;
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.GameEndEvent -= OnGameEndEvent;
        base.OnDisable();
    }

    private void OpenReturnPanel()
    {
        EventHandler.CallStopTimeEvent(true);
        UIManager.Instance.ShowPanel<ReturnPanel>();
    }

    private void OpenSettingPanel()
    {
        EventHandler.CallStopTimeEvent(true);
        UIManager.Instance.ShowPanel<SettingPanel>();
    }

    private void OnTimeChangeEvent(float nowTime, float maxTime)
    {
        sliderTime.value = Mathf.Clamp01(nowTime / maxTime);
    }

    private void OnScoreChangeEvent(int value)
    {
        score.text = value.ToString();
    }

    private void OnGameStateChangeEvent(GameState state)
    {
        SetMenuButtonsEnabled(state == GameState.Playing);
    }

    private void OnGameEndEvent(int finalScore, GameEndReason reason)
    {
        if (reason == GameEndReason.Aborted)
            return;

        EndPanel endPanel = UIManager.Instance.ShowPanel<EndPanel>();
        endPanel.SetScore(finalScore);
    }

    private void SetMenuButtonsEnabled(bool enabled)
    {
        btnReturn.interactable = enabled;
        btnSetting.interactable = enabled;
    }
}
