using System;
using System.Collections;
using UnityEngine;

public static class CorutineExtensions
{
    public static Coroutine WaitSecond(this MonoBehaviour obj, float seconds, Action action)
    {
        if (obj.gameObject.activeSelf)
            return obj.StartCoroutine(Timer(seconds, action));
        else
            return null;
    }

    public static Coroutine WaitRealSecond(this MonoBehaviour obj, float seconds, Action action)
    {
        if (obj.gameObject.activeSelf)
            return obj.StartCoroutine(TimerReal(seconds, action));
        else
            return null;
    }

    public static Coroutine UpdateCoroutine(this MonoBehaviour obj, Action action)
    {
        if (obj.gameObject.activeSelf)
            return obj.StartCoroutine(UpdateCoroutines(action));
        else
            return null;
    }

    public static void StopAll(this MonoBehaviour obj)
    {
        obj.StopAllCoroutines();
    }

    public static Coroutine WaitFrame(this MonoBehaviour obj, Action action)
    {
        if (obj.gameObject.activeSelf)
            return obj.StartCoroutine(Timer(action));
        else
            return null;
    }
    public static Coroutine WaitFrameFixed(this MonoBehaviour obj, Action action)
    {
        if (obj.gameObject.activeSelf)
            return obj.StartCoroutine(TimerFixed(action));
        else
            return null;
    }


    static IEnumerator Timer(Action action)
    {
        yield return new WaitForFixedUpdate();
        action?.Invoke();
    }

    static IEnumerator TimerFixed(Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }
    static IEnumerator Timer(float timer, Action action)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    static IEnumerator TimerReal(float timer, Action action)
    {
        yield return new WaitForSecondsRealtime(timer);
        action?.Invoke();
    }

    static IEnumerator UpdateCoroutines(Action action)
    {
        while (true)
        {
            action?.Invoke();
            yield return null;
        }
    }
}
