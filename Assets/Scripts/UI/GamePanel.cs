using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button btnReturn;
    public Button btnSetting;
    public Slider sliderTime;
    public override void Init()
    {
        btnReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<ReturnPanel>();
            //这里要实现游戏暂停逻辑
            EventHandler.CallStopTimeEvent(true);
        });
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
    }

    public override void UnInit()
    {
        btnReturn.onClick.RemoveAllListeners();
        btnSetting.onClick.RemoveAllListeners();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.TimeChangeEvent += OnTimeChangEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.TimeChangeEvent -= OnTimeChangEvent;
    }

    private void OnTimeChangEvent(float nowTime, float maxTime)
    {
        ChangeTime(nowTime / maxTime);
    }

    public void ChangeTime(float v)
    {
        sliderTime.value = v;
    }
}
