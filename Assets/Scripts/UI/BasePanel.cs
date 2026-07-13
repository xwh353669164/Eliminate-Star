using System;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup mCanvasGroup;
    private bool isShow;
    private float changeSpeed = 5f;

    private Action callBack;

    protected virtual void Awake()
    {
        mCanvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void OnEnable()
    {
        Init();
    }

    protected virtual void OnDisable()
    {
        UnInit();
    }

    protected virtual void Update()
    {
        if (isShow && mCanvasGroup.alpha != 1)
        {
            mCanvasGroup.alpha += changeSpeed * Time.deltaTime;
            if (mCanvasGroup.alpha >= 1)
                mCanvasGroup.alpha = 1;
        }
        else if (!isShow && mCanvasGroup.alpha != 0)
        {
            mCanvasGroup.alpha -= changeSpeed * Time.deltaTime;
            if (mCanvasGroup.alpha <= 0)
                mCanvasGroup.alpha = 0;

            callBack?.Invoke();
        }
    }
    /// <summary>
    /// 初始化控件
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// 销毁控件注册
    /// </summary>
    public abstract void UnInit();

    public virtual void ShowMe()
    {
        isShow = true;
        mCanvasGroup.alpha = 0f;
    }

    public virtual void HideMe(Action action)
    {
        isShow = false;
        mCanvasGroup.alpha = 1f;

        callBack = action;
    }
}
