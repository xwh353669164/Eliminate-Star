using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : BasePanel
{
    public Button btnLoad;
    public Button btnRegister;
    public InputField userNameInput;
    public InputField passwordInput;

    public override void Init()
    {
        btnLoad.onClick.AddListener(() =>
        {
            //进行校验

            //通过
            UIManager.Instance.HidePanel<LoadPanel>();
        });
        btnRegister.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<LoadPanel>();
            UIManager.Instance.ShowPanel<RegisterPanel>();
        });
    }

    public override void UnInit()
    {
        btnLoad.onClick.RemoveAllListeners();
        btnRegister.onClick.RemoveAllListeners();
        userNameInput.onEndEdit.RemoveAllListeners();
        passwordInput.onEndEdit.RemoveAllListeners();
    }
}
