using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        DontDestroyOnLoad(gameManager);

        UIManager.Instance.ShowPanel<BeginPanel>();
        UIManager.Instance.ShowPanel<LoadPanel>();
    }
}
