using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnBegin;
    public Button btnAbout;
    public Button btnSetting;
    public Button btnRank;

    public override void Init()
    {
        btnBegin.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<GamePanel>();
            UIManager.Instance.HidePanel<BeginPanel>();
            //通知游戏管理器，开始游戏了，要开始计时，随机生成星星预制体
            EventHandler.CallGameBeginEvent();
        });

        btnAbout.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<AboutPanel>();
        });

        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnRank.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<RankPanel>();
        });
    }

    public override void UnInit()
    {
        btnBegin.onClick.RemoveAllListeners();
        btnAbout.onClick.RemoveAllListeners();
        btnSetting.onClick.RemoveAllListeners();
        btnRank.onClick.RemoveAllListeners();
    }
}
