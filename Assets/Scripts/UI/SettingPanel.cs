using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
        });
        togMusic.onValueChanged.AddListener((v) =>
        {
            if (v) Debug.Log("打开音乐");
            else Debug.Log("关闭音乐");
        });
        togSound.onValueChanged.AddListener((v) =>
        {
            if (v) Debug.Log("打开音效");
            else Debug.Log("关闭音效");
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            Debug.Log("音乐滑动条:" + v);
        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            Debug.Log("音效滑动条:" + v);
        });
    }
    public override void UnInit()
    {
        btnClose.onClick.RemoveAllListeners();
    }
}
