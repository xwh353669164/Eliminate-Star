using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowPanel<BeginPanel>();
        UIManager.Instance.ShowPanel<LoadPanel>();

        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        DontDestroyOnLoad(gameManager);
    }
}
