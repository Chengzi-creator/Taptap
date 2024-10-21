using UnityEngine;
using System.Collections;
using System;


public delegate void ReduceAfterSeconds(VFX vFX);

public class DelayToInvoke : MonoBehaviour
{
    private static DelayToInvoke instance;
    public static DelayToInvoke Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("Prefab/DelayToInvoke")).GetComponent<DelayToInvoke>();
            }
            return instance;
        }
    }

    public static IEnumerator DelayToInvokeDo(ReduceAfterSeconds action,VFX vfx, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action(vfx);
    }

    public void StartDelayToInvokeDo(ReduceAfterSeconds reduceAfterSeconds,VFX vfx, float delaySeconds)
    {
        StartCoroutine(DelayToInvokeDo(reduceAfterSeconds, vfx, delaySeconds));
    }
}