using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public RectTransform board;
    private List<Vector2> boardGird = new List<Vector2>();

    private void OnEnable()
    {
        EventHandler.SwapnCreateStarEvent += SwapnCreateStar;
    }

    private void OnDisable()
    {
        EventHandler.SwapnCreateStarEvent -= SwapnCreateStar;
    }

    public void SwapnCreateStar()
    {
        Vector2 boardSize = board.rect.size;
        Vector2 cellSize = boardSize / 10;

        float startX = -boardSize.x / 2 + cellSize.x / 2;
        float startY = boardSize.y / 2 - cellSize.y / 2;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                float x = startX + (col * cellSize.x);
                float y = startY - (row * cellSize.y);
                boardGird.Add(new Vector2(x, y));
                int index = Random.Range(1, 5);
                GameObject star = ResManager.Instance.Load<GameObject>(Path.Combine("Prefabs", "Star" + index), board);
                star.transform.localPosition = new Vector2(x, y);
            }
        }
    }
}
