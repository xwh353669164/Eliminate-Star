using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    public Button btnAgain;
    public Button btnReturn;
    public Text score;
    public override void Init()
    {
        btnAgain.onClick.AddListener(() =>
        {

        });
        btnReturn.onClick.AddListener(() =>
        {

        });
    }

    public override void UnInit()
    {
        btnAgain.onClick.RemoveAllListeners();
        btnReturn.onClick.RemoveAllListeners();
    }
}
