using UnityEngine;
using System.Collections;
using System;


public delegate void ReduceAfterSeconds(VFX vFX);
public delegate void InvokeCreateVFXLianjuAfterSeconds(Vector2Int position, int attackDistance, 
    int faceDirection, int color = 7,bool firstInvoke = true);

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
    public static IEnumerator DelayToInvokeDo(InvokeCreateVFXLianjuAfterSeconds action, Vector2Int position, int attackDistance, int faceDirection, int color, float delaySeconds,bool firstInvoke)
    {
        yield return new WaitForSeconds(delaySeconds);
        action(position, attackDistance, faceDirection, color, firstInvoke);
    }

    public void StartDelayToInvokeDo(ReduceAfterSeconds reduceAfterSeconds,VFX vfx, float delaySeconds)
    {
        StartCoroutine(DelayToInvokeDo(reduceAfterSeconds, vfx, delaySeconds));
    }

    public void StartDelayToInvokeDo(InvokeCreateVFXLianjuAfterSeconds func, Vector2Int position, int attackDistance, int faceDirection, int color, float delaySeconds,bool firstInvoke)
    {
        StartCoroutine(DelayToInvokeDo(func, position, attackDistance, faceDirection, color, delaySeconds, firstInvoke));
    }
}