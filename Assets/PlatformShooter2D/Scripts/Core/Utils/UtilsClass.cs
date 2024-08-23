using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    public static void RunAfterDelay(MonoBehaviour monoBehaviour, float delay, Action task)
    {
        monoBehaviour.StartCoroutine(RunAfterDelayRoutine(delay, task));
    }


    private static IEnumerator RunAfterDelayRoutine(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);

        task.Invoke();
    }
}
