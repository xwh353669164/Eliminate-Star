using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    private Dictionary<string, BasePanel> mPanel = new Dictionary<string, BasePanel>();
    private Transform mCanvas;
    private UIManager()
    {
        mCanvas = GameObject.FindObjectOfType<Canvas>().transform;
    }

    public T ShowPanel<T>() where T : BasePanel
    {
        string name = typeof(T).Name;

        if (mPanel.ContainsKey(name))
            return (T)mPanel[name];

        GameObject panelPrefab = ResManager.Instance.Load<GameObject>(Path.Combine("UI", name), mCanvas);
        T panel = panelPrefab.GetComponent<T>();
        panel.ShowMe();

        mPanel.Add(name, panel);
        return panel;
    }

    public void HidePanel<T>() where T : BasePanel
    {
        string name = typeof(T).Name;

        if (mPanel.ContainsKey(name))
        {
            T panel = (T)mPanel[name];
            panel.HideMe(() =>
            {
                GameObject.Destroy(panel.gameObject);
                mPanel.Remove(name);
            });
        }
    }
    public T GetPanel<T>() where T : BasePanel
    {
        string name = typeof(T).Name;

        if (mPanel.ContainsKey(name))
            return (T)mPanel[name];

        return null;
    }
}
