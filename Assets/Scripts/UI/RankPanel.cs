using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    private static readonly string[] RankSpriteNames =
    {
        "NumberOne",
        "NumberTwo",
        "NumberThree",
        "Other"
    };

    public Button btnClose;
    public Transform content;

    public override void Init()
    {
        btnClose.onClick.AddListener(() => UIManager.Instance.HidePanel<RankPanel>());
        RebuildRankList();
    }

    public override void UnInit()
    {
        btnClose.onClick.RemoveAllListeners();
    }

    private void RebuildRankList()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        List<RankRecord> records = LocalDataService.Instance.GetRanks();
        for (int index = 0; index < records.Count; index++)
            CreateRankItem(index, records[index]);
    }

    private void CreateRankItem(int index, RankRecord record)
    {
        GameObject item = ResManager.Instance.Load<GameObject>(Path.Combine("Prefabs", "Rank"), content);
        Transform background = item.transform.Find("bk");
        background.Find("rank").GetComponent<Text>().text = (index + 1).ToString();
        item.transform.Find("score").GetComponent<Text>().text = record.score.ToString();
        item.transform.Find("time").GetComponent<Text>().text = record.GetDisplayTime();

        int spriteIndex = Mathf.Min(index, RankSpriteNames.Length - 1);
        background.GetComponent<Image>().sprite = ResManager.Instance.Load<Sprite>(
            Path.Combine("Sprite", RankSpriteNames[spriteIndex]));
    }
}
