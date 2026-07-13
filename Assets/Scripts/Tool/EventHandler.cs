using System;
public class EventHandler
{
    public static event Action GameBeginEvent;
    public static void CallGameBeginEvent()
    {
        GameBeginEvent?.Invoke();
    }

    public static event Action<float, float> TimeChangeEvent;
    public static void CallTimeChangeEvent(float nowTime, float maxTime)
    {
        TimeChangeEvent?.Invoke(nowTime, maxTime);
    }

    public static event Action<bool> StopTimeEvent;
    public static void CallStopTimeEvent(bool stop)
    {
        StopTimeEvent?.Invoke(stop);
    }

    public static event Action SwapnCreateStarEvent;
    public static void CallSwapnCreateStarEvent()
    {
        SwapnCreateStarEvent?.Invoke();
    }
}
