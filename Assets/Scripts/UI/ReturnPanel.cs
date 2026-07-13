using UnityEngine.UI;

public class ReturnPanel : BasePanel
{
    public Button btnContinue;
    public Button btnReturn;

    public override void Init()
    {
        btnContinue.onClick.AddListener(ContinueGame);
        btnReturn.onClick.AddListener(ReturnToBegin);
    }

    public override void UnInit()
    {
        btnContinue.onClick.RemoveAllListeners();
        btnReturn.onClick.RemoveAllListeners();
    }

    private void ContinueGame()
    {
        UIManager.Instance.HidePanel<ReturnPanel>();
        EventHandler.CallStopTimeEvent(false);
    }

    private void ReturnToBegin()
    {
        EventHandler.CallGameAbortEvent();
        UIManager.Instance.HidePanel<ReturnPanel>();
        UIManager.Instance.HidePanel<GamePanel>();
        UIManager.Instance.ShowPanel<BeginPanel>();
    }
}
