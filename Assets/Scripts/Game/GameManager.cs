using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxTime = 120f;
    private float nowTime;

    private bool isStop;


    private void OnEnable()
    {
        EventHandler.GameBeginEvent += OnGameBeginEvent;
        EventHandler.StopTimeEvent += OnStopTimeEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameBeginEvent -= OnGameBeginEvent;
        EventHandler.StopTimeEvent -= OnStopTimeEvent;
    }


    private void Update()
    {
        if(nowTime > 0 && !isStop)
        {
            nowTime -= Time.deltaTime;
            EventHandler.CallTimeChangeEvent(nowTime,maxTime);
        }
        if(nowTime == 0)
        {
            //游戏结束

        }
    }

    private void OnGameBeginEvent()
    {
        //随机生成星星预制体
        EventHandler.CallSwapnCreateStarEvent();
        //开始倒计时
        nowTime = maxTime;
    }

    private void OnStopTimeEvent(bool stop)
    {
        isStop = stop;
    }
}
