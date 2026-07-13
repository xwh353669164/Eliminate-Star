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
        btnClose.onClick.AddListener(ClosePanel);
        togMusic.onValueChanged.AddListener(OnMusicToggleChanged);
        togSound.onValueChanged.AddListener(OnSoundToggleChanged);
        sliderMusic.onValueChanged.AddListener(OnMusicVolumeChanged);
        sliderSound.onValueChanged.AddListener(OnSoundVolumeChanged);
    }

    public override void UnInit()
    {
        btnClose.onClick.RemoveAllListeners();
        togMusic.onValueChanged.RemoveAllListeners();
        togSound.onValueChanged.RemoveAllListeners();
        sliderMusic.onValueChanged.RemoveAllListeners();
        sliderSound.onValueChanged.RemoveAllListeners();
    }

    private void ClosePanel()
    {
        UIManager.Instance.HidePanel<SettingPanel>();
        EventHandler.CallStopTimeEvent(false);
    }

    private void OnMusicToggleChanged(bool enabled)
    {
        Debug.Log(enabled ? "打开音乐" : "关闭音乐");
    }

    private void OnSoundToggleChanged(bool enabled)
    {
        Debug.Log(enabled ? "打开音效" : "关闭音效");
    }

    private void OnMusicVolumeChanged(float value)
    {
        Debug.Log("音乐音量：" + value);
    }

    private void OnSoundVolumeChanged(float value)
    {
        Debug.Log("音效音量：" + value);
    }
}
