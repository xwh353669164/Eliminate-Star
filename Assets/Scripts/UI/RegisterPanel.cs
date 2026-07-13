using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    public Button btnClose;
    public Button btnRegister;
    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            CloseSlef();
        });
        btnRegister.onClick.AddListener(() =>
        {
            //进行校验

            //如果注册成功
            CloseSlef();
        });
    }

    public override void UnInit()
    {
        btnClose.onClick.RemoveAllListeners();
        btnRegister.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 隐藏自己打开登录面板
    /// </summary>
    private void CloseSlef()
    {
        UIManager.Instance.HidePanel<RegisterPanel>();
        UIManager.Instance.ShowPanel<LoadPanel>();
    }
}
