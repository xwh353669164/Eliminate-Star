using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnPanel : BasePanel
{
    public Button btnContinue;
    public Button btnReturn;

    public override void Init()
    {
        btnContinue.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ReturnPanel>();
            //这里要实现游戏时间恢复逻辑
            EventHandler.CallStopTimeEvent(false);
        });
        btnReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ReturnPanel>();
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.ShowPanel<BeginPanel>();
        });
    }

    public override void UnInit()
    {
        btnContinue.onClick.RemoveAllListeners();
        btnReturn.onClick.RemoveAllListeners();
    }
}
